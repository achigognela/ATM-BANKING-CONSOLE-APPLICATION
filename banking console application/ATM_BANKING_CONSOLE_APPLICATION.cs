using NLog;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Threading;
using Newtonsoft.Json.Schema;
using System.Transactions;

namespace BANKING_APPLICATION
{
    public class ATM_BANKING_CONSOLE_APPLICATION
    {
        public static baratis_mflobelis_monacemebi Validation()
        {
            string jsonFilePath = "C:\\Users\\achig\\source\\repos\\banking console application\\banking console application\\cardInfo.json";
            try
            {
                baratis_mflobelis_monacemebi[] userData = LoadUserData(jsonFilePath);

                if (userData != null)
                {
                    baratis_mflobelis_monacemebi validatedUser = ValidateCardInformation(userData);

                    if (validatedUser != null)
                    {
                        Console.WriteLine($"\nHello {validatedUser.firstName} {validatedUser.lastName}: \n");
                        return validatedUser; // Exit the program after successful validation
                    }

                    Console.WriteLine("Too many unsuccessful attempts! Wait for 15 seconds and try again.");
                    Thread.Sleep(15000);
                    Validation();
                }
            }
            catch (Exception ex)
            {
                // get a Logger object and log exception here using NLog. 
                // this will use the "fileLogger" logger from our NLog.config file
                Logger logger = LogManager.GetLogger("fileLogger");

                // add custom message and pass in the exception
                logger.Error(ex, $"Error: {ex.Message}");
            }

            return null;
        }
        static baratis_mflobelis_monacemebi[] LoadUserData(string jsonFilePath)
        {
            try
            {
                if (File.Exists(jsonFilePath))
                {
                    string jsonContent = File.ReadAllText(jsonFilePath);
                    return JsonConvert.DeserializeObject<baratis_mflobelis_monacemebi[]>(jsonContent);
                }

                Console.WriteLine("File not found: " + jsonFilePath);
            }
            catch (Exception ex)
            {
                // get a Logger object and log exception here using NLog. 
                // this will use the "fileLogger" logger from our NLog.config file
                Logger logger = LogManager.GetLogger("fileLogger");

                // add custom message and pass in the exception
                logger.Error(ex, $"Error: {ex.Message}");
            }
            return null;
        }
        static baratis_mflobelis_monacemebi ValidateCardInformation(baratis_mflobelis_monacemebi[] userData)
        {
            const int maxAttempts = 2;
            try
            {
                for (int attempts = 0; attempts < maxAttempts; attempts++)
                {
                    Console.Write("sheiyvanet baratis nomeri: ");
                    string cardNumber = Console.ReadLine();

                    Console.Write("sheiyvanet baratis CVC: ");
                    string cvc = Console.ReadLine();

                    Console.Write("sheiyvanet baratis moqmedebis vada (MM/YY): ");
                    string expirationDate = Console.ReadLine();

                    foreach (var user in userData)
                    {
                        if (IsValidCardInformation(user, cardNumber, cvc, expirationDate) && ValidatePIN(user))
                        {
                            return user;
                        }
                    }

                    Console.WriteLine("baratis araswori monacemebi. kidev scadet.\n");
                }
            }
            catch (Exception ex)
            {
                // get a Logger object and log exception here using NLog. 
                // this will use the "fileLogger" logger from our NLog.config file
                Logger logger = LogManager.GetLogger("fileLogger");

                // add custom message and pass in the exception
                logger.Error(ex, $"Error: {ex.Message}");
            }
            return null;
        }
        static bool IsValidCardInformation(baratis_mflobelis_monacemebi data, string cardNumber, string cvc, string expirationDate)
        {
            return cardNumber == data.cardDetails.cardNumber &&
                   cvc == data.cardDetails.CVC &&
                   expirationDate == data.cardDetails.expirationDate;
        }
        static bool ValidatePIN(baratis_mflobelis_monacemebi data)
        {
            const int maxPinAttempts = 3;
            try
            {
                for (int pinAttempts = 0; pinAttempts < maxPinAttempts; pinAttempts++)
                {
                    Console.Write("sheiyvanet baratis PIN KODI: ");
                    string enteredPin = Console.ReadLine();

                    if (IsValidPIN(data, enteredPin))
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("araswori PIN kodi. tavidan scadet.");
                    }
                }
            }
            catch (Exception ex)
            {
                // get a Logger object and log exception here using NLog. 
                // this will use the "fileLogger" logger from our NLog.config file
                Logger logger = LogManager.GetLogger("fileLogger");

                // add custom message and pass in the exception
                logger.Error(ex, $"Error: {ex.Message}");
            }

            return false;
        }
        static bool IsValidPIN(baratis_mflobelis_monacemebi data, string enteredPin)
        {
            return enteredPin == data.pinCode;
        }
        public static void Menu(baratis_mflobelis_monacemebi data)
        {
            Console.WriteLine("1. Nashtis naxva");
            Console.WriteLine("2. Tanxis gamotana angarishidan");
            Console.WriteLine("3. Bolo 5 operacia");
            Console.WriteLine("4. Tanxis shetana");
            Console.WriteLine("5. Pin kodis shecvla");
            Console.WriteLine("6. Tanxis konvertacia");
            Console.WriteLine("7. Exit\n");

            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    CheckDeposit(data);
                    Menu(data);
                    break;
                case "2":
                    GetAmount(data);
                    var newUser = Validation();
                    Menu(newUser);
                    break;
                case "3":
                    GetTransactionHistory(data);
                    Menu(data);
                    break;
                case "4":
                    AddAmount(data);
                    var newUser1 = Validation();
                    Menu(newUser1);
                    break;
                case "5":
                    ChangePin(data);
                    Menu(data);
                    break;
                case "6":
                    ChangeAmount(data);
                    Menu(data);
                    break;
                case "7":
                    var newUser2 = Validation();
                    Menu(newUser2);
                    break;
                default:
                    Console.WriteLine("Invalid input \n");
                    Menu(data);
                    break;
            }
        }
        static void Writejsontransaction(baratis_mflobelis_monacemebi data, Transaction transaction, string type)
        {
            try
            {
                DateTime currentUtcTime = DateTime.UtcNow.AddHours(4);
                string formattedTime = currentUtcTime.ToString("yyyy-MM-ddTHH:mm:ssZ");

                // Assuming you have a CardholderData object with new transaction data
                baratis_mflobelis_monacemebi newData = new baratis_mflobelis_monacemebi
                {
                    firstName = data.firstName,
                    lastName = data.lastName,
                    cardDetails = new baratis_monacmebi
                    {
                        cardNumber = data.cardDetails.cardNumber,
                        expirationDate = data.cardDetails.expirationDate,
                        CVC = data.cardDetails.CVC
                    },
                    pinCode = data.pinCode,
                    transactionHistory = new Transaction[]
                    {
                    new Transaction
                    {
                    transactionDate = DateTime.UtcNow.AddHours(4),
                    transactionType = type,
                    amount = transaction.amount,
                    amountGEL = transaction.amountGEL,
                    amountUSD = transaction.amountUSD,
                    amountEUR = transaction.amountEUR
                    }
                    }
                };

                // Specify the file path
                var filePath = "C:\\Users\\achig\\source\\repos\\banking console application\\banking console application\\cardInfo.json";

                // Read existing data from the file
                string jsonContent = File.ReadAllText(filePath);
                baratis_mflobelis_monacemebi[] existingData = JsonConvert.DeserializeObject<baratis_mflobelis_monacemebi[]>(jsonContent);

                // Find the cardholder data to update
                baratis_mflobelis_monacemebi existingCardholder = Array.Find(existingData, card => card.cardDetails.cardNumber == newData.cardDetails.cardNumber);

                if (existingCardholder != null)
                {
                    existingCardholder.pinCode = newData.pinCode;
                    // Convert array to a list, add the new transaction, and convert back to an array
                    List<Transaction> updatedTransactions = new List<Transaction>(existingCardholder.transactionHistory);
                    updatedTransactions.InsertRange(0, newData.transactionHistory);
                    existingCardholder.transactionHistory = updatedTransactions.ToArray();
                }

                // Write the updated data back to the file
                string updatedJsonContent = JsonConvert.SerializeObject(existingData, Formatting.Indented);
                File.WriteAllText(filePath, updatedJsonContent);
            }
            catch (Exception ex)
            {
                // get a Logger object and log exception here using NLog. 
                // this will use the "fileLogger" logger from our NLog.config file
                Logger logger = LogManager.GetLogger("fileLogger");

                // add custom message and pass in the exception
                logger.Error(ex, $"Error: {ex.Message}");
            }
        }
        static Transaction CheckDeposit(baratis_mflobelis_monacemebi data)
        {
            Console.WriteLine("\nAmount is");
            try
            {
                foreach (var transaction in data.transactionHistory)
                {

                    Console.WriteLine($"Amount (GEL): {transaction.amountGEL}");
                    Console.WriteLine($"Amount (USD): {transaction.amountUSD}");
                    Console.WriteLine($"Amount (EUR): {transaction.amountEUR}");
                    Console.WriteLine();

                    Writejsontransaction(data, transaction, "Deposit Check");
                    return transaction; // Exit the loop after displaying Balance

                }
            }
            catch (Exception ex)
            {
                // get a Logger object and log exception here using NLog. 
                // this will use the "fileLogger" logger from our NLog.config file
                Logger logger = LogManager.GetLogger("fileLogger");

                // add custom message and pass in the exception
                logger.Error(ex, $"Error: {ex.Message}");
            }
            return null;
        }
        static void GetAmount(baratis_mflobelis_monacemebi data)
        {
            try
            {
                Console.WriteLine("sheiyvanet gamosatani tanxis raodenoba an miutitet 'Exit' sistemidan gamosasvlelad:");
                var x = Console.ReadLine();
                if (x == "Exit" || x == "exit" || x == "EXIT")
                {
                    return;
                }
                else
                {
                    decimal.TryParse(x, out decimal amount);
                    if (amount > 0)
                    {
                        foreach (var transaction in data.transactionHistory)
                        {
                            if (transaction.amountGEL - amount < 0)
                            {
                                Console.WriteLine("ar gaqvt sakmarisi Tanxa angarishze.");
                                GetAmount(data);
                                return;

                            }
                            else
                            {
                                transaction.amount = amount;
                                transaction.amountGEL = transaction.amountGEL - amount;
                                Writejsontransaction(data, transaction, "Get Amount");
                           
                                    return;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Tqven sheiyvaneT arasakmarisi Tanxa!");
                        GetAmount(data);
                    }
                }
            }
            catch (Exception ex)
            {
                // get a Logger object and log exception here using NLog. 
                // this will use the "fileLogger" logger from our NLog.config file
                Logger logger = LogManager.GetLogger("fileLogger");

                // add custom message and pass in the exception
                logger.Error(ex, $"Error: {ex.Message}");
            }
        }
        static void GetTransactionHistory(baratis_mflobelis_monacemebi user)
        {
            Console.WriteLine("\nTransaction History:\n");

            var counter = 0;
            try
            {
                foreach (var transaction in user.transactionHistory)
                {
                    Console.WriteLine($"Transaction Date: {transaction.transactionDate}");
                    Console.WriteLine($"Transaction Type: {transaction.transactionType}");
                    Console.WriteLine($"Amount : {transaction.amount}");
                    Console.WriteLine($"Amount (GEL): {transaction.amountGEL}");
                    Console.WriteLine($"Amount (USD): {transaction.amountUSD}");
                    Console.WriteLine($"Amount (EUR): {transaction.amountEUR}");
                    Console.WriteLine();
                    counter++;

                    if (counter == 5 || counter == user.transactionHistory.Length)
                    {
                        foreach (var trans in user.transactionHistory)
                        {
                            Writejsontransaction(user, trans, "miiReT tranzaqciebis istoria");
                            return; // Exit the loop after displaying 5 transactions
                        }
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                // get a Logger object and log exception here using NLog. 
                // this will use the "fileLogger" logger from our NLog.config file
                Logger logger = LogManager.GetLogger("fileLogger");

                // add custom message and pass in the exception
                logger.Error(ex, $"Error: {ex.Message}");
            }
        }
        static void AddAmount(baratis_mflobelis_monacemebi data)
        {
            Console.WriteLine("sheiyvanet angarishze shesatani tanxis raodenoba an daachire 'Exit' sistemidan gamosasvlelad:\n");
            try
            {
                var x = Console.ReadLine();
                if (x == "Exit" || x == "exit" || x == "EXIT")
                {
                    return;
                }
                else
                {
                    decimal.TryParse(x, out decimal amount);
                    amount = Math.Round(amount, 2);
                    if (amount > 0)
                    {
                        foreach (var transaction in data.transactionHistory)
                        {
                            transaction.amount = amount;
                            transaction.amountGEL = transaction.amountGEL + amount;
                            Writejsontransaction(data, transaction, "Fill Amount");
                            Console.WriteLine($"tanxis raodenoba {amount} warmatebit daemata angarishze\n");
                            return;
                        }
                    }
                    else
                    {
                        Console.WriteLine("araswori raodenobis tanxa!");
                        AddAmount(data);
                    }
                    return;
                }
            }
            catch (Exception ex)
            {
                // get a Logger object and log exception here using NLog. 
                // this will use the "fileLogger" logger from our NLog.config file
                Logger logger = LogManager.GetLogger("fileLogger");

                // add custom message and pass in the exception
                logger.Error(ex, $"Error: {ex.Message}");
            }
        }
        static void ChangePin(baratis_mflobelis_monacemebi data)
        {
            Console.WriteLine("Please enter 4 digit new Pin Code or enter 'Exit' to exit: \n");
            try
            {
                var x = Console.ReadLine();
                if (x == "Exit" || x == "exit" || x == "EXIT")
                {
                    return;
                }
                else
                {
                    int.TryParse(x, out int pinCode);
                    if (pinCode.ToString().Length != 4)
                    {
                        ChangePin(data);
                    }
                    Console.WriteLine("Please Reenter new Pin Code\n");
                    int.TryParse(Console.ReadLine(), out int pinCode1);
                    if (pinCode == pinCode1)
                    {
                        data.pinCode = pinCode.ToString();
                        foreach (var transaction in data.transactionHistory)
                        {
                            Writejsontransaction(data, transaction, "Change Pin");
                            Console.WriteLine("\nPIN Code changed successfully!\n");
                            return;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid Pin Code has been entered!");
                        ChangePin(data);
                    }
                    return;
                }
            }
            catch (Exception ex)
            {
                // get a Logger object and log exception here using NLog. 
                // this will use the "fileLogger" logger from our NLog.config file
                Logger logger = LogManager.GetLogger("fileLogger");

                // add custom message and pass in the exception
                logger.Error(ex, $"Error: {ex.Message}");
            }
            return;
        }
        static void ChangeAmount(baratis_mflobelis_monacemebi data)
        {
            Console.WriteLine("Please enter the number of Currency you want to convert:\n");
            Console.WriteLine("1. GEL to USD");
            Console.WriteLine("2. GEL to EUR");
            Console.WriteLine("3. USD to GEL");
            Console.WriteLine("4. EUR to GEL");
            Console.WriteLine("5. USD to EUR");
            Console.WriteLine("6. EUR to USD");
            Console.WriteLine("7. Back to Menu");
            var num = Console.ReadLine();
            var exchangeRateUSD = 2.7m;
            var exchangeRateEUR = 3.00m;
            var exchangeRateUSDtoEUR = 0.91m;
            var exchangeRateEURtoUSD = 1.10m;
            try
            {
                switch (num)
                {
                    case "1":
                        Console.WriteLine("Please enter the amount of money to convert GEL into USD\n");
                        decimal.TryParse(Console.ReadLine(), out decimal exchange);
                        exchange = Math.Round(exchange, 2);
                        if (exchange > 0)
                        {
                            foreach (var transaction in data.transactionHistory)
                            {
                                if (exchange < transaction.amountGEL)
                                {
                                    transaction.amount = exchange;
                                    transaction.amountGEL = transaction.amountGEL - exchange;
                                    transaction.amountUSD = Math.Round(transaction.amountUSD + exchange / exchangeRateUSD, 2);
                                    Writejsontransaction(data, transaction, "Change Amount");
                                    Console.WriteLine("\nThe amount has succesfuly converted\n");
                                    return;
                                }
                                else
                                    Console.WriteLine("\nThe amount you entered is more than the amount available in your account\n");
                                ChangeAmount(data);
                                return;
                            }
                        }
                        Console.WriteLine("\ninvalid input\n");
                        ChangeAmount(data);
                        return;
                    case "2":
                        Console.WriteLine("Please enter the amount of money to convert GEL into EUR\n");
                        decimal.TryParse(Console.ReadLine(), out decimal exchange1);
                        exchange1 = Math.Round(exchange1, 2);
                        if (exchange1 > 0)
                        {
                            foreach (var transaction in data.transactionHistory)
                            {
                                if (exchange1 < transaction.amountGEL)
                                {
                                    transaction.amount = exchange1;
                                    transaction.amountGEL = transaction.amountGEL - exchange1;
                                    transaction.amountEUR = Math.Round(transaction.amountEUR + exchange1 / exchangeRateEUR, 2);
                                    Writejsontransaction(data, transaction, "Change Amount");
                                    Console.WriteLine("\nThe amount has succesfuly converted\n");
                                    return;
                                }
                                else
                                    Console.WriteLine("\nThe amount you entered is more than the amount available in your account\n");
                                ChangeAmount(data);
                                return;

                            }
                        }
                        Console.WriteLine("\ninvalid input\n");
                        ChangeAmount(data);
                        return;
                    case "3":
                        Console.WriteLine("Please enter the amount of money to convert USD into GEL\n");
                        decimal.TryParse(Console.ReadLine(), out decimal exchange2);
                        exchange2 = Math.Round(exchange2, 2);
                        if (exchange2 > 0)
                        {
                            foreach (var transaction in data.transactionHistory)
                            {
                                if (exchange2 < transaction.amountUSD)
                                {
                                    transaction.amount = exchange2;
                                    transaction.amountUSD = transaction.amountUSD - exchange2;
                                    transaction.amountGEL = Math.Round(transaction.amountGEL + exchange2 * exchangeRateUSD, 2);
                                    Writejsontransaction(data, transaction, "Change Amount");
                                    Console.WriteLine("\nThe amount has succesfuly converted\n");
                                    return;
                                }
                                else
                                    Console.WriteLine("\nThe amount you entered is more than the amount available in your account\n");
                                ChangeAmount(data);
                                return;
                            }
                        }
                        Console.WriteLine("\ninvalid input\n");
                        ChangeAmount(data);
                        return;
                    case "4":
                        Console.WriteLine("Please enter the amount of money to convert EUR into GEL\n");
                        decimal.TryParse(Console.ReadLine(), out decimal exchange3);
                        exchange3 = Math.Round(exchange3, 2);
                        if (exchange3 > 0)
                        {
                            foreach (var transaction in data.transactionHistory)
                            {
                                if (exchange3 < transaction.amountEUR)
                                {
                                    transaction.amount = exchange3;
                                    transaction.amountEUR = transaction.amountEUR - exchange3;
                                    transaction.amountGEL = Math.Round(transaction.amountGEL + exchange3 * exchangeRateEUR, 2);
                                    Writejsontransaction(data, transaction, "Change Amount");
                                    Console.WriteLine("\nThe amount has succesfuly converted\n");
                                    return;
                                }
                                else
                                    Console.WriteLine("\nThe amount you entered is more than the amount available in your account\n");
                                ChangeAmount(data);
                                return;
                            }
                        }
                        Console.WriteLine("\ninvalid input\n");
                        ChangeAmount(data);
                        return;
                    case "5":
                        Console.WriteLine("Please enter the amount of money to convert USD into EUR\n");
                        decimal.TryParse(Console.ReadLine(), out decimal exchange4);
                        exchange4 = Math.Round(exchange4, 2);
                        if (exchange4 > 0)
                        {
                            foreach (var transaction in data.transactionHistory)
                            {
                                if (exchange4 < transaction.amountUSD)
                                {
                                    transaction.amount = exchange4;
                                    transaction.amountUSD = transaction.amountUSD - exchange4;
                                    transaction.amountEUR = Math.Round(transaction.amountEUR + exchange4 * exchangeRateUSDtoEUR, 2);
                                    Writejsontransaction(data, transaction, "Change Amount");
                                    Console.WriteLine("\nThe amount has succesfuly converted\n");
                                    return;
                                }
                                else
                                    Console.WriteLine("\nThe amount you entered is more than the amount available in your account\n");
                                ChangeAmount(data);
                                return;
                            }
                        }
                        Console.WriteLine("\ninvalid input\n");
                        ChangeAmount(data);
                        return;
                    case "6":
                        Console.WriteLine("Please enter the amount of money to convert EUR into USD\n");
                        decimal.TryParse(Console.ReadLine(), out decimal exchange5);
                        exchange5 = Math.Round(exchange5, 2);
                        if (exchange5 > 0)
                        {
                            foreach (var transaction in data.transactionHistory)
                            {
                                if (exchange5 < transaction.amountEUR)
                                {
                                    transaction.amount = exchange5;
                                    transaction.amountEUR = transaction.amountEUR - exchange5;
                                    transaction.amountUSD = Math.Round(transaction.amountUSD + exchange5 * exchangeRateEURtoUSD, 2);
                                    Writejsontransaction(data, transaction, "Change Amount");
                                    Console.WriteLine("\nThe amount has succesfuly converted\n");
                                    return;
                                }
                                else
                                    Console.WriteLine("\nThe amount you entered is more than the amount available in your account\n");
                                ChangeAmount(data);
                                return;
                            }
                        }
                        Console.WriteLine("\ninvalid input\n");
                        ChangeAmount(data);
                        return;
                    case "7":
                        Menu(data);
                        return;
                    default:
                        Console.WriteLine("\nWrong input\n");
                        ChangeAmount(data);
                        break;
                }
            }
            catch (Exception ex)
            {
                // get a Logger object and log exception here using NLog. 
                // this will use the "fileLogger" logger from our NLog.config file
                Logger logger = LogManager.GetLogger("fileLogger");

                // add custom message and pass in the exception
                logger.Error(ex, $"Error: {ex.Message}");
            }
        }
    }
}