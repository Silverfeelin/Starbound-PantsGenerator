﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutfitGenerator
{
    class SleevesMerger
    {
        private const int SLEEVES_WIDTH = 387;
        private const int SLEEVES_HEIGHT = 301;
        private static Size sleevesSize = new Size(SLEEVES_WIDTH, SLEEVES_HEIGHT);

        public static void Generate(string[] args)
        {
            Bitmap result = null;

            if (args.Length != 2)
                Program.WaitAndExit("Improper usage! Expected parameters: <image_path_1> <image_path_2>\n" +
                            "Try dragging your image files directly on top of the application!");

            result = Sleeves(args[0], args[1]);
            string name = "mergedSleeves" + DateTime.Now.ToString(" MM.dd h.mm.ss") + ".png";
            result.Save(name);
            Program.WaitAndExit("Done saving, check \"" + name + "\"");
            return;
        }

        private static Bitmap Sleeves(string firstPath, string secondPath)
        {
            Bitmap frontSleeves;
            Bitmap backSleeves;

            Console.WriteLine("Is the order correct?");
            Console.WriteLine("front sleeve image: " + firstPath);
            Console.WriteLine("back sleeve image: " + secondPath);
            Console.WriteLine("Press Enter if it is, press any key otherwise");

            try
            {
                if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                {
                    frontSleeves = new Bitmap(firstPath);
                    backSleeves = new Bitmap(secondPath);
                }
                else
                {
                    frontSleeves = new Bitmap(secondPath);
                    backSleeves = new Bitmap(firstPath);
                }
            }
            catch (ArgumentException)
            {
                Program.WaitAndExit($"The file \"{firstPath}\"  or \"{secondPath}\" is not a valid image or does not exist.");
                return null;
            }

            if (!Generator.ValidSheet(frontSleeves, sleevesSize) || !Generator.ValidSheet(frontSleeves, sleevesSize))
            {
                Program.WaitAndExit("Incorrent size!");
                return null;
            }

            return ApllyMultingSleeves(frontSleeves, backSleeves);
        }

        private static Bitmap ApllyMultingSleeves(Bitmap frontSleeves, Bitmap backSleeves)
        {
            Bitmap result = new Bitmap(SLEEVES_WIDTH, SLEEVES_HEIGHT * 2);

            Superimpose(ref result, frontSleeves, 0, 0);
            Superimpose(ref result, backSleeves, 0, SLEEVES_HEIGHT);

            return result;
        }

        private static void Superimpose(ref Bitmap largeBmp, Bitmap smallBmp, int x, int y)
        {
            Graphics g = Graphics.FromImage(largeBmp);
            g.DrawImage(smallBmp, x, y, smallBmp.Width, smallBmp.Height);
        }
    }
}
