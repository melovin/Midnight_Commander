using Midnight_Commander.Components;
using Midnight_Commander.PopUps;
using Midnight_Commander.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Midnight_Commander.Windows
{
    public class Browser
    {
        //┌┐─├│┤└┘
        public int PaddFromLeft { get; set; }
        protected List<IComponent> components = new List<IComponent>();
        public string CurrentPath { get; set; }
        int selected = 0;
        int browserHeight = 22;
        int top = 0;        
        public string tempPath = "";
        private Stack<string> history = new Stack<string>();

        private List<string> HelpMenu = new List<string>();
        public IPopUp ActivePopUp { get; set; }
        
        public Browser(int padd)
        {
            PaddFromLeft = padd;
            CurrentPath = @"C:\";
            SetData(CurrentPath);
            this.HelpMenu.Add("DiskCh");
            this.HelpMenu.Add("Nápověda");
            this.HelpMenu.Add("------");
            this.HelpMenu.Add("Upravit");
            this.HelpMenu.Add("Kopie");
            this.HelpMenu.Add("Přesunout");
            this.HelpMenu.Add("Nová složka");
            this.HelpMenu.Add("Smazat");
            this.HelpMenu.Add("Nový soubor");
            this.HelpMenu.Add("Konec");
            Console.CursorVisible = false;
        }

        private void SetData(string path) //prijma seznamy slozek a souboru a uklada je jako komponenty
        {
                this.components.Clear();
                CurrentPath = path;
                components.Add(new LevelUp());
                foreach (DirectoryInfo item in DriveService.Directories(path))
                {
                    Folder folder = new Folder(item.Name, item.FullName, item.LastWriteTime.ToString("M/MMM HH:mm"), item.Parent.ToString());
                    this.components.Add(folder);    
                }
                foreach (FileInfo item in DriveService.Files(path))
                {
                    Files file = new Files(item.Name, item.FullName, item.Length.ToString(), item.LastWriteTime.ToString("M/MMM HH:mm"), item.DirectoryName.ToString());
                    this.components.Add(file);                   
                }
        }
        public void HandleKey(ConsoleKeyInfo info)
        {
            if(this.ActivePopUp != null && this.ActivePopUp.Active)
                    this.ActivePopUp.HandleKey(info);
            else
            {
                switch (info.Key)
                {
                    case ConsoleKey.DownArrow:
                        if (selected < this.components.Count - 1)
                        {
                            this.selected++;
                            if (this.selected == this.top + this.browserHeight)
                                this.top++;
                            Application.Draw();
                        }
                        return;
                    case ConsoleKey.UpArrow:
                        if (selected > 0)
                        {
                            this.selected--;
                            if (this.selected == this.top - 1)
                                this.top--;
                            Application.Draw();
                        }
                        return;

                    case ConsoleKey.Tab:
                        if (BrowsersW.LeftIsCurrent)
                            BrowsersW.LeftIsCurrent = false;
                        else
                            BrowsersW.LeftIsCurrent = true;
                        Application.Draw();
                        return;

                    case ConsoleKey.Enter:
                        if (components[selected].Name.Contains('/'))
                        {
                            if (selected == 0 && this.history.Count != 0) //vraceni se ze slozek
                                this.SetData(this.history.Pop());
                            else if (selected != 0) //leze do slozek
                            {
                                CurrentPath = components[selected].FullName;
                                tempPath = this.components[selected].Parent;
                                history.Push(tempPath);
                                this.SetData(CurrentPath);
                            }
                            selected = 0;
                            top = 0;
                            Application.Draw();
                        }
                        return;
                    case ConsoleKey.Escape:
                        Console.WindowHeight = 30;
                        Console.WindowWidth = 120;
                        Console.Clear();
                        Application.Draw();
                        return;
                    case ConsoleKey.F1: //změna disku 
                        DriveChanger driveChanger = new DriveChanger();
                        this.ActivePopUp = driveChanger;
                        driveChanger.ChangeDrive += DriveChanger_ChangeDrive;
                        driveChanger.Draw();
                        return;
                    case ConsoleKey.F2:
                        Help help = new Help();
                        this.ActivePopUp = help;
                        help.Draw();
                        return;
                    case ConsoleKey.F4: //editor
                        if(!this.components[selected].Name.Contains('/'))
                        {
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.Clear();
                            Application.Window = new Editor(this.components[selected].Name, this.components[selected].FullName,this.CurrentPath);
                            Application.Draw();
                        }
                        return;
                    case ConsoleKey.F5: //kopirovani 
                        Copier copier = new Copier(this.components[selected].FullName, this.components[selected].Name);
                        if (BrowsersW.LeftIsCurrent)
                            copier.SetPath(BrowsersW.RightBPath);
                        else
                            copier.SetPath(BrowsersW.LeftBPath);
                        this.ActivePopUp = copier;
                        copier.Draw();
                        return;
                    case ConsoleKey.F6: //přesun 
                        Mover mover = new Mover(this.components[selected].FullName, this.components[selected].Name);
                        if (BrowsersW.LeftIsCurrent)
                            mover.SetPath(BrowsersW.RightBPath);
                        else
                            mover.SetPath(BrowsersW.LeftBPath);
                        this.ActivePopUp = mover;
                        mover.Draw();
                        selected = 0;
                        top = 0;
                        return;
                    case ConsoleKey.F7: //nova slozka 
                        FolderAdder adder = new FolderAdder();
                        this.ActivePopUp = adder;
                        adder.FolderName += Adder_FolderName;
                        adder.Draw();
                        return;
                    case ConsoleKey.F8:  //smazat 
                        Deleter deleter = new Deleter(this.components[selected].FullName,this.components[selected].Name.Substring(2));
                        this.ActivePopUp = deleter;
                        deleter.Draw();
                        if (Deleter.Confirmed)
                            this.components.RemoveAll(item => item.Name == this.components[selected].Name);
                        selected = 0;
                        top = 0;
                        return;
                    case ConsoleKey.F9:
                        NewFile newFile = new NewFile();
                        this.ActivePopUp = newFile;
                        newFile.nameOfFile += NewFile_nameOfFile;
                        newFile.Draw();
                        return;
                    case ConsoleKey.F10:
                        Console.Clear();
                        Console.SetCursorPosition(Console.WindowWidth / 2 - 5, Console.WindowHeight / 2);
                        Console.WriteLine("KONEC...");
                        Console.ReadKey();
                        Environment.Exit(1);
                        return;
                    default:
                        this.Draw();
                        return;
                }
            }

        }

        private void NewFile_nameOfFile(string path)
        {
            string tempPath = "";
            if (CurrentPath.Length > 3)
                tempPath = CurrentPath + @"\" + path;
            else
                tempPath = CurrentPath + path;
            using (FileStream fs = File.Create(tempPath))
            Application.Draw();
        }

        private void Adder_FolderName(string path) //pouziji vracene jmeno slozky a vytvorim ji
        {
            string tempPath = "";
            if (CurrentPath.Length > 3)
                tempPath = CurrentPath +@"\" +  path;
            else
                tempPath = CurrentPath + path;
            Directory.CreateDirectory(tempPath);
            Application.Draw();
        }

        private void DriveChanger_ChangeDrive(string path)
        {
            this.CurrentPath = path;
            this.SetData(path);
            selected = 0;
            Console.ForegroundColor = ConsoleColor.White;
            Application.Draw();
        }

        public void Draw()
        {
            this.SetData(CurrentPath);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            string tempPath = this.CurrentPath;
            if (tempPath.Length > Console.WindowWidth / 2 - 10)
                tempPath = tempPath.Substring(0, Console.WindowWidth / 2 - 10) + "..";
            Console.SetCursorPosition(PaddFromLeft, 0);

            for (int i = 0; i <= Console.WindowWidth / 2 - tempPath.Length; i++) //prvni radek a aktualni cesta
            {
                if (i == 0)
                    Console.Write("┌");
                else if (i == 3)
                {                   
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(tempPath);
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else if (i == Console.WindowWidth / 2 - tempPath.Length)
                    Console.Write("┐");
                else
                    Console.Write("─");
            }
            Console.SetCursorPosition(PaddFromLeft, 1);
            Console.Write("│".PadRight(Console.WindowWidth / 4 - 18)+ "Název".PadRight(22)+ "│ Velikost │".PadRight(5)+ "    Úprava   ".PadRight(6)+ "│"); //vykresli prvni 2 radky
            Console.SetCursorPosition(PaddFromLeft, 2);
            int j = 3;
            for (int i = this.top; i < this.top + this.browserHeight; i++) //vykresluje obsah tabulky
            {
                string str = "│";
                if (this.selected == i && ((BrowsersW.LeftInProcess && BrowsersW.LeftIsCurrent) ||  (!BrowsersW.LeftInProcess && !BrowsersW.LeftIsCurrent))) //zabarvuje vybraný radek
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                if (i > this.components.Count - 1) //dokresluje prazdne misto v tabulce
                {
                    Console.Write(str.PadRight(34));
                    Console.Write(str.PadRight(11));
                    Console.Write(str.PadRight(14));
                    Console.WriteLine(str);
                }
                else
                    this.components[i].Draw(); //obsah
                Console.SetCursorPosition(PaddFromLeft, j);
                j++;
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.Write("├".PadRight(Console.WindowWidth / 2 - 1, '─') + "┤");
            Console.SetCursorPosition(PaddFromLeft, this.browserHeight + 3);

            Console.Write(new string(' ', Console.WindowWidth / 2)); //vypisuje dolu jméno složky, na které aktuálně stojím
            Console.SetCursorPosition(PaddFromLeft, this.browserHeight + 3);
            if (selected == 0)
                Console.Write("│Level up".PadRight(Console.WindowWidth/2-this.components[selected].Name.Length+3));
            else if(this.components.Count>1)
                Console.Write(this.components[selected].Name);
            Console.SetCursorPosition(PaddFromLeft+ Console.WindowWidth / 2 - 1, this.browserHeight + 3);
            Console.WriteLine('│');
            Console.SetCursorPosition(PaddFromLeft, this.browserHeight + 4);
            Console.Write("└".PadRight(32, '─'));

            Console.Write(DriveService.Drive(CurrentPath.Substring(0,3))+"GB".PadRight(5)); //Vypisuje informace o disku
            Console.Write(DriveService.Total.ToString().Substring(0,5) + "%".PadRight(15, '─') + '┘');
        }
        public void DrawHelpMenu() //Vykresluje dolní menu
        {
            Console.SetCursorPosition(PaddFromLeft, this.browserHeight + 7);
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
