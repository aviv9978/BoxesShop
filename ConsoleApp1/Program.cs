using BoxesDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Manager m1 = new Manager(2, 5, 3);
            //Manager m1 = new Manager();
            m1.AddNewBoxToStorage(2.5, 5, 5);
            m1.AddNewBoxToStorage(2.5, 6, 5);
            m1.AddNewBoxToStorage(3.1, 6, 7);
            m1.AddNewBoxToStorage(3.1, 5, 7);
            m1.AddNewBoxToStorage(2.3, 4, 10);
            m1.AddNewBoxToStorage(2.1, 5.5, 5);
            m1.AddNewBoxToStorage(2.5, 10, 10);
            m1.AddNewBoxToStorage(2.7, 5.5, 15);
            while (true)
            {
                while (true)
                {
                    Console.WriteLine("Hello! Here is our menu:");
                    Console.WriteLine("Enter 1 for deal offer and purchasing");
                    Console.WriteLine("Enter 2 for day past");
                    Console.WriteLine("Enter 3 to add new box to storage");
                    Console.WriteLine("Enter 4 to check box's quantity in storage");
                    Console.WriteLine("Enter 5 to add quantity for existed box in storage");

                    string readAnswer = Console.ReadLine();
                    int chosen;
                    bool valide = int.TryParse(readAnswer, out chosen);
                    if (valide)
                    {
                        if (chosen == 1)
                        {
                            string answersInOption1;
                            double width;
                            double height;
                            int quantity;
                            while (true)
                            {
                                Console.WriteLine("Please enter box width to buy");
                                answersInOption1 = Console.ReadLine();
                                while (!double.TryParse(answersInOption1, out width))
                                {
                                    Console.WriteLine("Please enter correct width to buy");
                                    answersInOption1 = Console.ReadLine();
                                }
                                break;
                            }
                            while (true)
                            {
                                Console.WriteLine("Please enter box height to buy");
                                answersInOption1 = Console.ReadLine();
                                while (!double.TryParse(answersInOption1, out height))
                                {
                                    Console.WriteLine("Please enter correct height to buy");
                                    answersInOption1 = Console.ReadLine();
                                }
                                break;
                            }
                            while (true)
                            {
                                Console.WriteLine("Please enter box quantity to buy");
                                answersInOption1 = Console.ReadLine();
                                while (!int.TryParse(answersInOption1, out quantity))
                                {
                                    Console.WriteLine("Please enter correct quantity to buy");
                                    answersInOption1 = Console.ReadLine();
                                }
                                break;
                            }
                            if (quantity == 0)
                            {
                                Console.WriteLine("Quantity must be above 0.\n");
                                break;
                            }
                            if (!m1.RequiredStorageValidation(width, height, quantity)) //Checking store validation for the required quantity
                            {
                                Console.WriteLine("Sorry. We don't have the required quantity in storage.");
                                Console.WriteLine($"{m1.AlternativeQuantityBoxOffer(width, height, quantity)}\n");
                                if (m1.AlternativeQuantityOffer == 0)
                                    break;
                                bool yesOrNo;
                                while (true)
                                {
                                    Console.WriteLine("Would you like to take this offer instead?");
                                    string value = Console.ReadLine();
                                    if (!bool.TryParse(value, out yesOrNo))
                                        Console.WriteLine("Please enter true or false");
                                    else break;
                                }
                                if (yesOrNo == true)
                                    Console.WriteLine($"\n{m1.FinalePurchase(width, height, m1.AlternativeQuantityOffer)}");
                                else Console.WriteLine("Have a good day!\n");
                            }
                            else //Enough quantity in storage for the required quantity
                            {
                                Console.WriteLine(m1.BoxOffer(width, height, quantity));
                                bool yesOrNo;
                                while (true)
                                {
                                    Console.WriteLine("Would you like to take this offer?");
                                    string value = Console.ReadLine();
                                    if (!bool.TryParse(value, out yesOrNo))
                                        Console.WriteLine("Please enter true or false");
                                    else break;
                                }
                                if (yesOrNo == true)
                                    Console.WriteLine($"\n{m1.FinalePurchase(width, height, quantity)}");
                                else Console.WriteLine("Have a good day!\n");
                            }

                        }
                        else if (chosen == 2)
                        {
                            Console.WriteLine(m1.DayPast());
                            break;
                        }
                        else if (chosen == 3)
                        {
                            string answersInOption3;
                            double width;
                            double height;
                            int quantity;
                            while (true)
                            {
                                Console.WriteLine("Please enter box width to add");
                                answersInOption3 = Console.ReadLine();
                                while (!double.TryParse(answersInOption3, out width))
                                {
                                    Console.WriteLine("Please enter correct width to add");
                                    answersInOption3 = Console.ReadLine();
                                }
                                break;
                            }
                            while (true)
                            {
                                Console.WriteLine("Please enter box height to add");
                                answersInOption3 = Console.ReadLine();
                                while (!double.TryParse(answersInOption3, out height))
                                {
                                    Console.WriteLine("Please enter correct height to add");
                                    answersInOption3 = Console.ReadLine();
                                }
                                break;
                            }
                            if (m1.DoesBoxExisted(width, height))
                                Console.WriteLine("Box already existed in storage.");
                            else
                            {
                                Console.WriteLine("Please enter box quantity to add");
                                answersInOption3 = Console.ReadLine();
                                while (!int.TryParse(answersInOption3, out quantity))
                                {
                                    Console.WriteLine("Please enter correct quantity to add");
                                    answersInOption3 = Console.ReadLine();
                                }
                                m1.AddNewBoxToStorage(width, height, quantity);
                                Console.WriteLine("Great! The box has been added\n");
                            }
                        }
                        else if (chosen == 4)
                        {
                            string answersInOption1;
                            double width;
                            double height;
                            while (true)
                            {
                                Console.WriteLine("Please enter box width for quantity check");
                                answersInOption1 = Console.ReadLine();
                                while (!double.TryParse(answersInOption1, out width))
                                {
                                    Console.WriteLine("Please enter correct width for quantity check");
                                    answersInOption1 = Console.ReadLine();
                                }
                                break;
                            }
                            while (true)
                            {
                                Console.WriteLine("Please enter box height for quantity check");
                                answersInOption1 = Console.ReadLine();
                                while (!double.TryParse(answersInOption1, out height))
                                {
                                    Console.WriteLine("Please enter correct height for quantity check");
                                    answersInOption1 = Console.ReadLine();
                                }
                                break;
                            }
                            if (m1.DoesBoxExisted(width, height))
                                Console.WriteLine($"Quantity for Box ({width},{height}) in storage:{m1.GetBoxQuantity(width, height)}\n");
                            else Console.WriteLine("Box doesn't existed.\n");
                        }
                        else if (chosen == 5)
                        {
                            string answerInOption5;
                            double width;
                            double height;
                            int quantity;
                            while (true)
                            {
                                Console.WriteLine("Please enter box width to add quantity");
                                answerInOption5 = Console.ReadLine();
                                while (!double.TryParse(answerInOption5, out width))
                                {
                                    Console.WriteLine("Please enter correct width to add quantity");
                                    answerInOption5 = Console.ReadLine();
                                }
                                break;
                            }
                            while (true)
                            {
                                Console.WriteLine("Please enter box height to add quantity");
                                answerInOption5 = Console.ReadLine();
                                while (!double.TryParse(answerInOption5, out height))
                                {
                                    Console.WriteLine("Please enter correct height to add quantity");
                                    answerInOption5 = Console.ReadLine();
                                }
                                break;
                            }
                            if (!m1.DoesBoxExisted(width, height))
                                Console.WriteLine("Box doesn't existed.\n");
                            else
                            {
                                Console.WriteLine("Please enter box quantity to add");
                                answerInOption5 = Console.ReadLine();
                                while (!int.TryParse(answerInOption5, out quantity))
                                {
                                    Console.WriteLine("Please enter correct quantity to add");
                                    answerInOption5 = Console.ReadLine();
                                }
                                if (quantity == 0)
                                    Console.WriteLine("Quantity must be above 0.\n");
                                else if (!m1.NewQuantityValidation(width, height, quantity))
                                    Console.WriteLine($"Sorry. Quantity can't be over the known limit({m1.GetMaxQuantityLimit()}).\n");

                                else
                                {
                                    m1.AddQuantity(width, height, quantity);
                                    Console.WriteLine($"{quantity} box(es) ({width},{height}) have been added\n");
                                }
                            }
                        }

                        else Console.WriteLine("Please enter a correct number");
                    }
                }
            }
        }
    }
}
