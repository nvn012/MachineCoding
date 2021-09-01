using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {

            User.AddDummyUsers();
            Balance.balanceManager = new Dictionary<KeyValuePair<string, string>, float>();
            while (true)
            {
                string input = Console.ReadLine();
                if (input == "STOP") break; 
                if(input == "SHOW")
                {
                    ShowAllUserBalances();
                    continue;
                }
                if(input.StartsWith("SHOW"))
                {
                    ShowUserData(input.Split(' ')[1]);
                    continue;
                }
                expenseType et;
                if (input.Contains(expenseType.EQUAL.ToString()))
                {
                    et = expenseType.EQUAL;
                }
                else if (input.Contains(expenseType.EXACT.ToString()))
                {
                    et = expenseType.EXACT;
                }
                else
                {
                    et = expenseType.PERCENT;
                }

                switch (et)
                {
                    case expenseType.EQUAL:
                        AddEqualExpense(input);
                        break;
                    case expenseType.EXACT:
                        AddEXACTExpense(input);
                        break;
                    case expenseType.PERCENT:
                        AddPercentExpense(input);
                        break;
                    default:
                        break;
                }
            }


        }

        private static void ShowUserData(string v)
        {
            User use = new User();
            User attendee = use.GetUserById(v);
            foreach (var rec in Balance.balanceManager)
            {
                if (rec.Key.Key == v || rec.Key.Value == v)
                {
                    if (rec.Value < 0)
                        Console.WriteLine(rec.Key.Key.ToString() + " OWES " + Math.Abs(rec.Value) + " To " + rec.Key.Value.ToString());
                    else if (rec.Value == 0) Console.WriteLine("No Balances");
                    else
                        Console.WriteLine(rec.Key.Value.ToString() + " OWES " + Math.Abs(rec.Value) + " To " + rec.Key.Key.ToString());
                }
            }
        }

        private static void ShowAllUserBalances()
        {
            Balance bal = new Balance();
            if (Balance.balanceManager.Count == 0) Console.WriteLine("No Balances");
            foreach (var rec in Balance.balanceManager)
            {
                if (rec.Value < 0)
                    Console.WriteLine(rec.Key.Key.ToString() + " OWES " + Math.Abs(rec.Value) + " To " + rec.Key.Value.ToString());
                else if (rec.Value == 0) continue;
                else
                    Console.WriteLine(rec.Key.Value.ToString() + " OWES " + Math.Abs(rec.Value) + " To " + rec.Key.Key.ToString());
            }
        }

        private static void AddPercentExpense(string input)
        {
            //stream u4 1200 4 u1 u2 u3 u4 PERCENT 40 20 20 20
            string[] inputRecord = input.Split(' ');
            User user = new User();
            User payee = user.GetUserById(inputRecord[0]);

            var expenseAmount = int.Parse(inputRecord[1]);
            var involvedUsers = int.Parse(inputRecord[2]);

            for (int i = 0; i < involvedUsers; i++)
            {
                if (payee.userId == user.GetUserById(inputRecord[3 + i]).userId)
                    continue;
                KeyValuePair<string, string> userMapping = new KeyValuePair<string, string>
                (
                    payee.userId, user.GetUserById(inputRecord[3 + i]).userId
                );

                UpdateBalance(userMapping, expenseAmount*(int.Parse(inputRecord[involvedUsers+4+i]))/100);
            }

        }

        private static void AddEXACTExpense(string input)
        {
            //stream u1 1250 2 u2 u3 EXACT 370 880
            string[] inputRecord = input.Split(' ');
            User user = new User();
            User payee = user.GetUserById(inputRecord[0]);

            var involvedUsers = int.Parse(inputRecord[2]);
            
            for (int i = 0; i < involvedUsers; i++)
            {
                if (payee.userId == user.GetUserById(inputRecord[3 + i]).userId)
                    continue;
                KeyValuePair<string, string> userMapping = new KeyValuePair<string, string>
                (
                    payee.userId, user.GetUserById(inputRecord[3 + i]).userId
                );

                UpdateBalance(userMapping, int.Parse(inputRecord[involvedUsers+4+i]));
            }

        }

        private static void AddEqualExpense(string input)
        {
            //stream u1 1000 4 u1 u2 u3 u4 EQUAL

            string[] inputRecord = input.Split(' ');
            User user = new User();
            User payee = user.GetUserById(inputRecord[0]);

            var expenseAmount = int.Parse(inputRecord[1]);
            var involvedUsers = int.Parse(inputRecord[2]);

            for (int i = 0; i < involvedUsers; i++)
            {
                if (payee.userId == user.GetUserById(inputRecord[3 + i]).userId)
                    continue;
                KeyValuePair<string, string> userMapping = new KeyValuePair<string, string>
                (
                    payee.userId,user.GetUserById(inputRecord[3+i]).userId
                );
                UpdateBalance(userMapping, expenseAmount / involvedUsers);
            }


        }

        private static void UpdateBalance(KeyValuePair<string, string> userMapping, int v)
        {
            var userBalances = Balance.balanceManager;
           if(!userBalances.ContainsKey(userMapping))
            {
                bool userComboPresent = false;
                foreach (KeyValuePair<string,string> keyValuePair in userBalances.Keys)
                {
                    if(keyValuePair.Value == userMapping.Key && keyValuePair.Key==userMapping.Value)
                    {
                        var tmpMappingg = new KeyValuePair<string, string>(userMapping.Value,userMapping.Key);
                        userBalances[tmpMappingg] -= v;
                        userComboPresent = true;
                        break;
                    }
                }
                if(!userComboPresent)
                {
                    userBalances.Add(userMapping, v);
                }
            }
           else
            {
                userBalances[userMapping] += v;
            }
        }

        enum expenseType
        {
            EQUAL,
            EXACT,
            PERCENT
        }
    }


 

    
  

}
