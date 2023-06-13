using System;
using System.IO;
using System.Collections;
class HangmanGame
{
    static void Main()
    {
        HangmanGame game = new HangmanGame();
        game.Run();
    }
    private int maxGuesses;
    private int guessesRemaining;
    private string word;
    private ArrayList guessedLetters;
    private string hiddenWord;
    private bool gameOver;
    public HangmanGame()
    {
        maxGuesses = 4;
        guessesRemaining = maxGuesses;
        word = "";
        guessedLetters = new ArrayList();
        hiddenWord = "";
        gameOver = false;
    }
    public void Run()
    {
        InitializeGame();
        while (!gameOver)
        {
            DisplayHangman();
            DisplayGameStatus();
            ProcessGuess();
            CheckGameStatus();
        }
        Console.WriteLine("Stiskněte libovolnou klávesu pro ukončení...");
        Console.ReadKey();
    }
    private void InitializeGame()
    {
        using (StreamReader sr = new StreamReader("soubor.txt"))
        {
            int count = 0;
            while (sr.ReadLine() != null)
                count++;

            sr.BaseStream.Seek(0, SeekOrigin.Begin);
            var rnd = new Random();
            int randomLine = rnd.Next(1, count + 1);

            for (int i = 1; i <= randomLine; i++)
                word = sr.ReadLine();
        }

        hiddenWord = new String('*', word.Length);
    }
    private void DisplayHangman()
    {
        Console.Clear();
        string[,] board = new string[8, 8];

        if (guessesRemaining == maxGuesses)
        {
            board = new string[8, 8]{
                {" "," "," "," "," "," "," "," ",},
                {" "," "," "," "," "," "," "," ",},
                {" "," "," "," "," ",""," "," "},
                {" "," "," "," "," "," "," "," ",},
                {" "," "," "," "," "," "," "," "},
                {" "," "," "," "," "," "," "," ",},
                {" ","#","#","#","#","#","#"," ",},
                {"#","#","#","#","#","#","#","#",},
            };
        }
        else if (guessesRemaining == maxGuesses - 1)
        {
            board = new string[8, 8]{
                {" "," "," "," "," "," "," "," ",},
                {" "," "," ","|"," "," "," "," ",},
                {" "," "," ","|"," ",""," ",""},
                {" "," "," ","|"," "," "," "," ",},
                {" "," "," ","|"," "," "," ",""},
                {" "," "," ","|"," "," "," "," ",},
                {" ","#","#","#","#","#","#"," ",},
                {"#","#","#","#","#","#","#","#",},
            };
        }
        else if (guessesRemaining == maxGuesses - 2)
        {
            board = new string[8, 8]{
                {" "," "," ","_","_","_"," "," ",},
                {" "," "," ","|"," "," "," "," ",},
                {" "," "," ","|"," ",""," ",""},
                {" "," "," ","|"," "," "," "," ",},
                {" "," "," ","|"," "," "," ",""},
                {" "," "," ","|"," "," "," "," ",},
                {" ","#","#","#","#","#","#"," ",},
                {"#","#","#","#","#","#","#","#",},
            };
        }
        else if (guessesRemaining == maxGuesses - 3)
        {
            board = new string[8, 8]{
                {" "," "," ","_","_","_"," "," ",},
                {" "," "," ","|"," ","|"," "," ",},
                {" "," "," ","|"," ","o"," ",""},
                {" "," "," ","|"," "," "," "," ",},
                {" "," "," ","|"," "," "," ",""},
                {" "," "," ","|"," "," "," "," ",},
                {" ","#","#","#","#","#","#"," ",},
                {"#","#","#","#","#","#","#","#",},
            };
        }
        else if (guessesRemaining <= maxGuesses - 4)
        {
            board = new string[8, 8]{
                {" "," "," ","_","_","_"," "," ",},
                {" "," "," ","|"," ","|"," "," ",},
                {" "," "," ","|","\\_o_/",""," ",""},
                {" "," "," ","|"," ","|"," "," ",},
                {" "," "," ","|","_/","\\_"," ",""},
                {" "," "," ","|"," "," "," "," ",},
                {" ","#","#","#","#","#","#"," ",},
                {"#","#","#","#","#","#","#","#",},
            };
        }
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Console.Write(board[i, j] + " ");
            }
            Console.WriteLine();
        }
    }
    private void DisplayGameStatus()
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("   ŠIBENICE");
        Console.WriteLine("---------------");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Slovo: " + hiddenWord);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Zbývající pokusy: " + guessesRemaining);
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.Write("Uhodnutá písmena: ");
        for (int i = 0; i < guessedLetters.Count; i++)
        {
            Console.Write(guessedLetters[i] + " ");
        }
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("Zadejte písmeno: ");
        Console.ForegroundColor = ConsoleColor.White;

    }
    private void ProcessGuess()
    {
        char guess = Console.ReadLine().ToLower()[0];

        if (guessedLetters.Contains(guess))
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Toto písmeno jste již zkusili!");
            Console.WriteLine("Stiskněte libovolnou klávesu pro pokračování...");
            Console.ForegroundColor = ConsoleColor.White;

            Console.ReadKey();
            return;
        }
        guessedLetters.Add(guess);

        if (word.Contains(guess))
        {
            for (int i = 0; i < word.Length; i++)
            {
                if (word[i] == guess)
                    hiddenWord = hiddenWord.Remove(i, 1).Insert(i, guess.ToString());
            }
        }
        else
        {
            guessesRemaining--;
        }
    }
    private void CheckGameStatus()
    {
        if (hiddenWord == word)
        {
            Console.WriteLine();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Gratulujeme! Uhodli jste slovo "+ word + " správně!");
            Console.ForegroundColor = ConsoleColor.White;
            gameOver = true;
        }
        else if (guessesRemaining == 0)
        {
            Console.WriteLine();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Došly vám pokusy! Hledané slovo bylo: " + word);
            gameOver = true;
        }
    }
}