using Midnight_Commander.Components;
using Midnight_Commander.PopUps;
using Midnight_Commander.PopUps.EditorPopUps;
using Midnight_Commander.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Midnight_Commander.Windows
{
    public class Editor : Window
    {
        private string Name { get; set; }
        private string FilePath { get; set; }
        private bool Edited = false;
        private int CurrentRow = 0;
        private int CurrentChar = 0;
        private int NumberOfChars { get; set; }
        private List<string> content { get; set; }
        private int CursorLeft = 0;
        private int Top = 0;
        private List<string> HelpMenu = new List<string>();
        public IPopUp ActivePopUp { get; set; }
        private string PathOfBrowser { get; set; }
        private int LeftTop = 0;
        private bool MarkText = false;
        private string MarkedText = "";
        private int MarkedFrom { get; set; }
        private int MarkedTo { get; set; }
        private int MarkedRow { get; set; }
        public int SearchFrom = 0;
        private List<int> MarkedRows = new List<int>();
        public Editor(string name, string path, string pathOfBrowser)
        {
            this.Name = name.Substring(1);
            this.FilePath = path;
            this.content = FileService.FileContent(this.FilePath);
            this.HelpMenu.Add("Nápověda");
            this.HelpMenu.Add("Uložit");
            this.HelpMenu.Add("Označ");
            this.HelpMenu.Add("Nahraď");
            this.HelpMenu.Add("Kopie");
            this.HelpMenu.Add("Přesunout");
            this.HelpMenu.Add("Hledat");
            this.HelpMenu.Add("Smazat");
            this.HelpMenu.Add("Hl.nabídka");
            this.HelpMenu.Add("Konec");
            foreach (string item in this.content) //pocitam pocet charu
            {
                this.NumberOfChars += item.Length + 1;
            }
            if(this.content.Count != 0)
                this.NumberOfChars--;
            this.PathOfBrowser = pathOfBrowser;
        }
        public override void HandleKey(ConsoleKeyInfo info)
        {

            if (this.ActivePopUp != null && this.ActivePopUp.Active)
                this.ActivePopUp.HandleKey(info);
            else
            {
                switch (info.Key)
                {
                    case ConsoleKey.RightArrow:
                        this.RightArrow();
                        return;
                    case ConsoleKey.LeftArrow:
                        this.LeftArrow();
                        return;
                    case ConsoleKey.UpArrow:
                        this.UpArrow();
                        return;
                    case ConsoleKey.DownArrow:
                        this.DownArrow();
                        return;
                    case ConsoleKey.Backspace:
                        this.Backspace();
                        return;
                    case ConsoleKey.Enter:
                        this.Enter();
                        return;
                    case ConsoleKey.F2: //pouze ulozit
                        this.F2();
                        return;
                    case ConsoleKey.F3: //oznaceni textu
                        this.F3();
                        return;
                    case ConsoleKey.F4: //nahrazeni slova
                        this.F4();
                        return;
                    case ConsoleKey.F5: //copy
                        this.Copy();
                        return;
                    case ConsoleKey.F6: //move
                        this.Move();
                        return;
                    case ConsoleKey.F7: //hledani textu
                        this.FindText();
                        return;
                    case ConsoleKey.F8: //delete
                        this.Delete();
                        return;
                    case ConsoleKey.F10: //dialog pro odchod
                        this.F10();
                        return;
                    default:
                        this.Default(info);
                        return;
                }
            }
        }
        private void RightArrow()
        {
            //Console.Clear();
            if (this.CurrentRow + this.Top != this.content.Count && this.CurrentChar != this.NumberOfChars) //jet jen do konce souboru
            {
                if (this.CursorLeft < this.content[CurrentRow + this.Top].Length && this.CursorLeft != Console.WindowWidth - 1)
                {
                    if (CurrentRow == MarkedRow)
                    {
                        if (this.MarkText && this.CursorLeft >= this.MarkedFrom)
                        {
                            this.MarkedText += this.content[CurrentRow + this.Top][this.CursorLeft].ToString();
                            //MarkedFrom = this.CursorLeft;
                        }
                        else if (this.MarkText && this.MarkedText != "" && this.CursorLeft < this.MarkedFrom)
                            this.MarkedText = this.MarkedText.Remove(0, 1);
                    }
                    else
                    {
                        if (MarkText)
                        {
                            this.MarkedTo++;
                            this.MarkedText += this.content[CurrentRow + this.Top][this.CursorLeft].ToString();
                        }
                            
                    }
                    //if (this.MarkText && this.CursorLeft >= this.MarkedFrom)
                    //{
                    //    this.MarkedText += this.content[CurrentRow + this.Top][this.CursorLeft].ToString();
                    //    //MarkedFrom = this.CursorLeft;
                    //}
                    //else if (this.MarkText && this.MarkedText != "" && this.CursorLeft < this.MarkedFrom)
                    //    this.MarkedText = this.MarkedText.Remove(0, 1);
                    this.CursorLeft++;
                    this.CurrentChar++;
                }
                else if (this.CurrentRow == Console.WindowHeight - 3 && this.CursorLeft == this.content[CurrentRow + this.Top].Length && this.CursorLeft != Console.WindowWidth - 1) //jsem na spodku konzole
                {
                    this.Top++;
                    this.CursorLeft = 0;
                    this.CurrentChar++;
                }
                else if (this.CursorLeft == Console.WindowWidth - 1 && this.CursorLeft + this.LeftTop < this.content[CurrentRow + this.Top].Length - 1) //posouvani celeho radku doprava
                    this.LeftTop++;
                else //jsem na konci radku
                {
                    if (this.MarkText)
                    {
                        this.MarkedText += "\n";
                        //MarkedFrom = 0;
                        this.MarkedRows.Add(this.CurrentRow + 1);
                    }
                    this.CurrentChar++;
                    this.CurrentRow++;
                    this.CursorLeft = 0;
                }
                this.Draw();
            }
        }
        private void LeftArrow()
        {
            if (this.CurrentRow != 0 && this.CursorLeft == 0 && this.CursorLeft + this.LeftTop == 0) //radek nahoru
            {
                this.CurrentRow--;
                if (this.MarkText)
                {
                    this.MarkedRows.RemoveAt(this.MarkedRows.Count - 1);
                    //kdyz mam oznaceno nahore a jdu dolu
                    if (this.MarkedRows.Contains(this.CurrentRow))
                        this.MarkedText = this.MarkedText.Remove(this.MarkedText.Length - 1, 1);
                    //kdyz mam oznaceno dole a jdu nahoru
                   else
                    this.MarkedText = this.MarkedText.Insert(0, "\n");

                }
                this.CurrentChar--;
                this.CursorLeft = Math.Min(this.content[CurrentRow + this.Top].Length, Console.WindowWidth - 1);
                this.LeftTop = this.content[CurrentRow + this.Top].Length - Math.Min(this.content[CurrentRow + this.Top].Length, Console.WindowWidth - 1);
            }
            else if (this.MarkText && this.CursorLeft == 0 && this.CurrentRow == 0)
                this.MarkedText = "";
            else if (this.CurrentRow == 0 && this.Top != 0) //skok nahoru - posunutí konzole
            {
                this.Top--;
                if (this.MarkText)
                    this.MarkedText += "\n";
                this.CurrentChar--;
                this.CursorLeft = Math.Min(this.content[CurrentRow + this.Top].Length, Console.WindowWidth - 1);
                this.LeftTop = this.content[CurrentRow + this.Top].Length - Math.Min(this.content[CurrentRow + this.Top].Length, Console.WindowWidth - 1);
            }
            else if (this.CursorLeft == 0 && this.CursorLeft + this.LeftTop != 0) //posouvani konzole doleva
                this.LeftTop--;
            else if (this.CursorLeft != 0) //obycejne posouvani doleva
            {
                this.CursorLeft--;
                this.CurrentChar--;
                //Console.Clear();
                if (this.MarkText && this.CursorLeft < this.MarkedFrom)
                {
                    this.MarkedText = this.MarkedText.Insert(0, this.content[CurrentRow + this.Top][this.CursorLeft].ToString());
                    this.MarkedTo--;
                }
                else if (this.MarkText && this.MarkedText != "")
                {
                    this.MarkedText = this.MarkedText.Remove(this.MarkedText.Length - 1);
                    this.MarkedTo--;
                }
                    
                //else if (this.MarkText && this.MarkedText == "")
                //    this.MarkedText += this.MarkedText.Insert(0, this.content[CurrentRow + this.Top][this.CursorLeft].ToString());


                //    this.MarkedText = this.MarkedText.Insert(0, this.content[CurrentRow + this.Top][this.CursorLeft].ToString());
                //else if(this.MarkText)
                //    this.MarkedText = this.content[CurrentRow + this.Top][this.CursorLeft].ToString();
            }

            this.Draw();
        }
        private void UpArrow()
        {
            if (this.CurrentRow > 0)
            {
                if (this.MarkText && this.MarkedText == "")
                {
                    this.MarkedText += this.content[CurrentRow + this.Top - 1].Substring(this.MarkedFrom, Math.Min(this.content[CurrentRow + this.Top - 1].Length - this.MarkedFrom, Console.WindowWidth)).ToString() + "\n";
                    this.MarkedText += this.content[CurrentRow + this.Top].Substring(0, this.MarkedFrom).ToString();
                    this.MarkedRows.Add(CurrentRow + this.Top - 1);
                }
                else if (this.MarkText && this.MarkedText != "" && !this.MarkedRows.Contains(this.CurrentRow - 1))
                {
                    this.MarkedText = this.MarkedText.Insert(0, this.content[CurrentRow + this.Top - 1].Substring(this.CursorLeft, Math.Min(this.content[CurrentRow + this.Top - 1].Length - this.CursorLeft, Console.WindowWidth)).ToString()
                        + "\n" + this.content[CurrentRow + this.Top].Substring(0, Math.Min(this.MarkedFrom, this.CursorLeft)).ToString());
                    this.MarkedRows.Add(CurrentRow + this.Top - 1);
                }
                else if (this.MarkText && this.MarkedText != "" && this.MarkedRows.Contains(this.CurrentRow - 1))
                {
                    this.MarkedText = this.MarkedText.Insert(0, this.content[CurrentRow + this.Top - 1].Substring(this.CursorLeft, Math.Min(this.content[CurrentRow + this.Top - 1].Length - this.CursorLeft, Console.WindowWidth)).ToString()
                        + "\n" + this.content[CurrentRow + this.Top].Substring(0, Math.Min(this.MarkedFrom, this.CursorLeft)).ToString());
                    
                }
                if (this.content[CurrentRow - 1].Length - 1 < this.CursorLeft)
                    this.CursorLeft = this.content[CurrentRow - 1].Length;
                if (this.CursorLeft > this.content[this.CurrentRow + this.Top].Length)
                    this.CursorLeft = Math.Min(this.content[CurrentRow + this.Top].Length, Console.WindowWidth - 1);
                this.CurrentRow--;

            }
            else if (this.CurrentRow == 0 && this.Top != 0)
                this.Top--;
            this.Draw();
        }
        private void DownArrow()
        {
            if (this.CurrentRow == Console.WindowHeight - 3 && this.CurrentRow + this.Top != this.content.Count - 1) //posouvani vrchniho radku
            {
                this.CurrentChar += this.content[CurrentRow + this.Top].Length - CursorLeft + 1;
                this.Top++;
                this.Draw();
            }
            else if (this.CurrentRow + this.Top != this.content.Count - 1) //prepinani radku
            {
                
                if (this.MarkText)
                {
                    this.MarkedText += this.content[CurrentRow + this.Top].Substring(Math.Min(this.CursorLeft,this.content[this.CurrentRow + this.Top].Length)) + "\n";
                    this.MarkedText += this.content[CurrentRow + this.Top + 1].Substring(0, Math.Min(this.MarkedTo, this.content[CurrentRow + this.Top + 1 ].Length));
                    this.MarkedRows.Add(this.CurrentRow + 1);
                }
                this.CurrentChar += this.content[CurrentRow + this.Top].Length - CursorLeft + 1;
                this.CurrentRow++;
                if (this.CursorLeft > this.content[this.CurrentRow + this.Top].Length)
                    this.CursorLeft = Math.Min(this.content[CurrentRow + this.Top].Length, Console.WindowWidth - 1);
                this.Draw();
            }
        }
        private void Backspace()
        {
            if (this.content[CurrentRow + this.Top].Length != 0 && this.CursorLeft <= this.content[CurrentRow + this.Top].Length && this.CursorLeft != 0) //maze pismeno
            {
                Edited = true;
                this.content[CurrentRow + this.Top] = this.content[CurrentRow + this.Top].Remove(CursorLeft - 1, 1);
                this.CursorLeft--;
                CurrentChar--;
                NumberOfChars--;
            }
            else if (this.CursorLeft == 0 && this.content[this.CurrentRow + this.Top] == "" && this.CurrentRow != 0) //zbavuje se prazdneho radku
            {
                this.content.RemoveAt(this.CurrentRow + this.Top);
                this.CurrentRow--;
                this.CursorLeft = this.content[CurrentRow + this.Top].Length;
            }
            else if (this.CursorLeft == 0 && this.content[this.CurrentRow + this.Top] != "" && this.CurrentRow != 0) //prilepi k hornimu radku
            {
                this.CursorLeft = this.content[CurrentRow + this.Top - 1].Length;
                this.content[this.CurrentRow + this.Top - 1] += this.content[this.CurrentRow + this.Top];
                this.content.RemoveAt(this.CurrentRow + this.Top);
                this.CurrentRow--;

            }
            else if (this.CursorLeft == 0 && this.CurrentRow + this.Top != 0) //kurzor nahoru za posledni char
            {
                this.CurrentRow--;
                this.CursorLeft = this.content[CurrentRow + this.Top].Length;
            }
            this.Draw();
        }
        private void Enter()
        {
            if (this.CursorLeft == this.content[CurrentRow + this.Top].Length) //kurzor dolu
            {
                this.CursorLeft = 0;
                this.CurrentRow++;
                this.content.Insert(this.CurrentRow + this.Top, "");
            }
            else if (this.content[this.CurrentRow + this.Top] != "") //zalamuje radek
            {
                this.content.Insert(this.CurrentRow + this.Top + 1, this.content[this.CurrentRow + this.Top].Substring(this.CursorLeft));
                this.content[this.CurrentRow + this.Top] = this.content[this.CurrentRow + this.Top].Substring(0, CursorLeft);
                this.CurrentRow++;
                this.CursorLeft = 0;
            }
            this.Draw();
        }
        private void F2()
        {
            if (Edited)
            {
                SaveChanges save = new SaveChanges(this.FilePath);
                save.YouCanSaveMe += Save_YouCanSaveMe;
                this.ActivePopUp = save;
                save.Draw();
            }
        }
        private void F3()
        {
            if (this.MarkText)
            {
                this.MarkedTo = this.CursorLeft;
                this.MarkText = false;
            }
            else
            {
                this.MarkText = true;
                this.MarkedText = "";
                MarkedFrom = this.CursorLeft;
                this.MarkedRow = this.CurrentRow;
                this.MarkedRows.Clear();
                this.MarkedRows.Add(this.CurrentRow);
            }
            this.Draw();
        }
        private void F4()
        {
            TextReplacer replacer = new TextReplacer();
            this.ActivePopUp = replacer;
            replacer.Replace += Replacer_Replace;
            replacer.Draw();
        }

        private void Replacer_Replace(string replaceMe) //vyhledavani pro replace + rozcestnik na jine funkce
        {
            string[] wordsToReplace = replaceMe.Split('>');
            this.MarkedRows.Clear();
            string[] words = null;
            this.MarkText = true;
            int tempCursorLeft = this.CursorLeft;
            for (int i = this.SearchFrom; i < this.content.Count; i++) //řádky
            {
                words = this.content[i].Split(' ');
                int len = 0;
                for (int j = 0; j < words.Length; j++) //slova
                {
                    if (words[j] == wordsToReplace[0])
                    {
                        this.MarkedText = this.content[i].Substring(len, wordsToReplace[0].Length);
                        this.MarkedRow = i;
                        this.MarkedRows.Add(i);
                        this.MarkedFrom = len;
                        this.CursorLeft = len + wordsToReplace[0].Length;
                        this.CurrentRow = i;
                        break;
                    }
                    else
                        len += words[j].Length + 1;
                }
                if (this.MarkedRows.Count != 0)
                    break;

            }
            this.Draw();
            this.MarkText = false;
            this.MarkedText = "";

            ConfirmatorForReplacement confirmator = new ConfirmatorForReplacement(wordsToReplace);
            this.ActivePopUp = confirmator;
            
            confirmator.Skip += Confirmator_Skip;
            confirmator.Replace += Confirmator_Replace;
            confirmator.ReplaceAll += Confirmator_ReplaceAll;
            confirmator.Draw();
        }

        private void Confirmator_ReplaceAll(string replaceMe)
        {
            string[] replaceThis = replaceMe.Split('>');
            this.MarkedRows.Clear();
            string[] words = null;
            this.MarkText = true;
            int tempCursorLeft = this.CursorLeft;
            for (int i = this.CurrentRow + this.Top; i < this.content.Count; i++) //řádky
            {
                words = this.content[i].Split(' ');
                int len = 0;
                for (int j = 0; j < words.Length; j++) //slova
                {
                    if (words[j] == replaceThis[0])
                    {
                        string tempRow = this.content[i].Substring(0,len);
                        this.content[i] = tempRow + this.content[i].Substring(len + replaceThis[0].Length);
                        this.CursorLeft = len;
                        this.content[i] = this.content[i].Insert(len, replaceThis[1]);
                        this.Edited = true;
                    }
                    else
                    {
                        len += words[j].Length + 1;
                    }
                }
                if (this.MarkedRows.Count != 0)
                    break;
                this.CurrentRow++;
                this.CursorLeft = len + replaceThis[1].Length;
            }
            this.CurrentRow = 0;
            this.CursorLeft = 0;
            this.Draw();
            this.MarkText = false;
            this.MarkedText = "";
        } //replace vseho

        private void Confirmator_Skip(string replaceMe)
        {
            this.SearchFrom = this.CurrentRow + 1;
            Replacer_Replace(replaceMe);
        } //preskakovani na dalsi stejny text

        private void Confirmator_Replace(string replaceMe,string replaceTo)
        {
            string tempStr = this.content[this.CurrentRow + this.Top].Substring(0,this.MarkedFrom);
            this.content[this.CurrentRow + this.Top] = tempStr + this.content[this.CurrentRow + this.Top].Substring(this.MarkedFrom + replaceMe.Length);
            this.CursorLeft = this.MarkedFrom;
            this.content[this.CurrentRow + this.Top] = this.content[this.CurrentRow + this.Top].Insert(this.MarkedFrom, replaceTo);
            this.Edited = true;
            this.Draw();
            this.SearchFrom++;
            this.Replacer_Replace(replaceMe + ">" + replaceTo);

        } //obycejny replace jednoho textu

        private void F10()
        {
            if (Edited) //ulozit pred ochodem nebo ne
            {
                SaveAndQuit saveAndQuit = new SaveAndQuit(this.Edited, this.FilePath, this.PathOfBrowser);
                saveAndQuit.YouCanSaveMe += SaveAndQuit_YouCanSaveMe;
                this.ActivePopUp = saveAndQuit;
                saveAndQuit.Draw();
            }
            else //nezmeneny pouze zavrit
            {
                Application.Window = Application.brws;
                if (BrowsersW.LeftIsCurrent)
                    BrowsersW.LeftBPath = this.PathOfBrowser;
                else
                    BrowsersW.RightBPath = this.PathOfBrowser;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();
                BrowsersW.FromEditor = true;
                Application.Draw();
            }
        }
        private void Default(ConsoleKeyInfo info)
        {
            try
            {
                if (this.content[CurrentRow + this.Top].Length < this.CursorLeft)
                {
                    this.content[CurrentRow + this.Top] += info.KeyChar.ToString();
                    this.CursorLeft++;
                }
                else
                {
                    this.content[CurrentRow + this.Top] = this.content[CurrentRow + this.Top].Insert(this.CursorLeft, info.KeyChar.ToString());
                    this.CursorLeft++;
                }
                if (this.MarkText)
                    this.MarkedText += info.KeyChar.ToString();
            }
            catch 
            {
                this.content.Insert(this.CursorLeft, info.KeyChar.ToString());
                this.CursorLeft++;
                if (this.MarkText)
                    this.MarkedText += info.KeyChar.ToString();
            }
            this.CurrentChar++;
            this.NumberOfChars++;
            Edited = true;
            this.Draw();

        }
        private void SaveAndQuit_YouCanSaveMe() //ulozeni a zavreni
        {
            FileService.SaveMe(this.FilePath, this.content);
        }

        private void Save_YouCanSaveMe() //ulozeni
        {
            FileService.SaveMe(this.FilePath, this.content);
            this.Edited = false;
        }
        private void Copy()
        {
            this.content[this.CurrentRow + this.Top] = this.content[this.CurrentRow + this.Top].Insert(this.CursorLeft, this.MarkedText);
            this.CursorLeft += this.MarkedText.Length;
            this.MarkedText = "";
            this.Edited = true;
            this.Draw();
        }
        private void Delete()
        {
            this.content[this.CurrentRow + this.Top] = this.content[this.CurrentRow + this.Top].Remove(Math.Min(this.CursorLeft, this.MarkedFrom), this.MarkedText.Length);
            this.CursorLeft = this.MarkedFrom;
            this.MarkedText = "";
            this.Edited = true;
            this.Draw();
        }
        private void Move()
        {
            this.content[this.CurrentRow + this.Top] = this.content[this.CurrentRow + this.Top].Insert(this.CursorLeft, this.MarkedText);
            this.content[this.MarkedRow] = this.content[this.MarkedRow].Remove(Math.Min(this.CursorLeft, this.MarkedFrom), this.MarkedText.Length);
            this.MarkedFrom = CursorLeft;
            this.MarkedText = "";
            this.Edited = true;
            this.Draw();
        }
        private void FindText()
        {
            TextFinder finder = new TextFinder();
            this.ActivePopUp = finder;
            finder.TextToFind += Finder_TextToFind;
            finder.Draw();           
        }

        private void Finder_TextToFind(string text) //hledání
        {
            this.MarkedRows.Clear();
            string[] words = null;
            this.MarkText = true;
            int tempCursorLeft = this.CursorLeft;
            for (int i = this.CurrentRow + this.Top; i < this.content.Count; i++) //řádky
            {
                words = this.content[i].Split(' ');
                int len = 0;
                for (int j = 0; j < words.Length; j++) //slova
                {
                    if (words[j] == text)
                    {
                            this.MarkedText = this.content[i].Substring(len, text.Length);
                            this.MarkedRow = i;
                            this.MarkedRows.Add(i);
                            this.MarkedFrom = len;
                            this.CursorLeft = len + text.Length;
                            this.CurrentRow = i;
                            break;
                    }
                    else
                        len = words[j].Length + 1;
                }
                if (this.MarkedRows.Count != 0)
                    break;
            }
            this.Draw();
            this.MarkText = false;
            this.MarkedText = "";
        }

        public void DrawTop() //lista s pocitanim
        {
            string ascii = "0010";
            try
            {
                ascii = Convert.ToInt32(this.content[this.CurrentRow + this.Top][this.CursorLeft]).ToString();
            }
            catch
            { }
            string hex = "00A";
            try
            {
                hex = Convert.ToInt32(this.content[this.CurrentRow + this.Top][this.CursorLeft]).ToString("x");
            }
            catch
            { }
            Console.SetCursorPosition(0, 0);
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(this.Name.PadRight(20));
            if (this.MarkText && this.Edited)
                Console.Write("[--MB-]".PadRight(9));
            else if (this.Edited)
                Console.Write("[--M--]".PadRight(9));
            else if (this.MarkText)
                Console.Write("[--B--]".PadRight(9));
            else
                Console.Write("[-----]".PadRight(9));
            Console.Write(this.CursorLeft + this.LeftTop);
            Console.WriteLine($" L:[{this.Top + 1}+{this.CurrentRow}  {this.CurrentRow + this.Top + 1}/{this.content.Count}] *({CurrentChar}/{this.NumberOfChars}) {ascii} 0x{hex}".PadRight(Console.WindowWidth - 30));
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
        }
        public override void Draw() //vypise obsah
        {
            if (MarkText)
                this.MarkedTo = CursorLeft;
            string[] markedLines = null;

            markedLines = this.MarkedText.Split("\n");
            this.DrawTop();
            int tempTo = this.MarkedTo;
            int tempFrom = this.MarkedFrom;
            if (this.CurrentRow == this.MarkedRow && this.MarkedTo < this.MarkedFrom)
            {
                tempTo = this.MarkedFrom;
                tempFrom = this.MarkedTo;
            }
            else if (this.CurrentRow < this.MarkedRow)
            {
                tempTo = this.MarkedFrom;
                tempFrom = this.MarkedTo;
            }
            Console.SetCursorPosition(0, 1);
            int index = 0;
            for (int i = this.Top; i < Math.Min(this.content.Count, Console.WindowHeight - 2) + Top; i++)
            {
                if (!this.MarkedRows.Contains(i)) // vypisuji obyčejný řádek
                    Console.WriteLine(this.content[i].Substring(this.LeftTop, Math.Min(this.content[i].Length - this.LeftTop, Console.WindowWidth)).PadRight(Console.WindowWidth));
                else if (this.MarkedText != "" && this.MarkedRows.Contains(i)) // --vypisuji řádek s označením
                {
                 //Zkontroluje jestli vypisuji první záznam z listu označených, jestli jo, tak se to snaží vypsat to před označeným textem
                    if (index == 0)
                        Console.Write(this.content[i].Substring(0, tempFrom));
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.Write(markedLines[index]); //vždy vypisu to oznacene na radku
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                 //Pokud je tam pouze jeden záznam - označený jen jeden řádek max
                    if (markedLines.Length == 1)
                    {
                        if (MarkedFrom == 0)
                            Console.WriteLine(this.content[i].Substring(markedLines[0].Length)); //, Math.Min(this.content[i].Length - this.MarkedText.Length, Console.WindowWidth)
                        else
                            Console.WriteLine(this.content[i].Substring(tempTo)); //, Math.Min(this.content[i].Length - this.MarkedText.Length, Console.WindowWidth)
                    }
                 //Vypíše zbytek po označeném řádku (Buď nic nebo ten zbytek)
                    else
                    {
                        if (index == 0)
                        {
                            Console.WriteLine();
                        }
                        else if (markedLines.Length - 1 == index)
                            Console.WriteLine(this.content[i].Substring(Math.Min(tempTo, this.content[CurrentRow + this.Top].Length)));                           
                        else
                            Console.WriteLine();
                    }
                    index++;
                }
                else if (this.content[i].Length <= this.LeftTop)
                    Console.WriteLine("".PadRight(Console.WindowWidth));

                else
                    Console.WriteLine(this.content[i].PadRight(Console.WindowWidth));
            }
            this.DrawHelpMenu();
            Console.SetCursorPosition(CursorLeft, CurrentRow + 1);
            Console.CursorVisible = true;
            Console.CursorSize = 3;
        }
        public void DrawHelpMenu() //Vykresluje dolní menu
        {
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            int n = 1;
            foreach (string item in this.HelpMenu)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write(n);
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write(item);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("   ");
                n++;
            }
        }
    }
}
