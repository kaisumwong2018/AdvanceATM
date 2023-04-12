
using AdvanceATM.Model;
using ConsoleTables;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using System.Text.RegularExpressions;

AppDbContext db = new AppDbContext();

bool logout = true;

while (logout)
{

    Console.Clear();

    #region Art

    Console.ForegroundColor = ConsoleColor.Yellow;

    Console.WriteLine(@"______  ___________  _____________________   _______ __
___   |/  /__    | \/ /__  __ )__    |__  | / /__  //_/
__  /|_/ /__  /| |_  /__  __  |_  /| |_   |/ /__  ,<   
_  /  / / _  ___ |  / _  /_/ /_  ___ |  /|  / _  /| |  
/_/  /_/  /_/  |_/_/  /_____/ /_/  |_/_/ |_/  /_/ |_|  
                                                       ");

    Console.ResetColor();

    Console.WriteLine();

    #endregion

    Console.WriteLine("1.Admin Login");

    Console.WriteLine("");

    Console.WriteLine("2.User Login");

    Console.WriteLine();

    Console.WriteLine("3.Exit");
    string opt = Console.ReadLine()!;

    switch (opt)
    {
        case "1":

            #region Login/Checking (Admin)

            Console.Clear();

            #region Art
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@"    \    _ \   \  | _ _|   \ |    |      _ \   __| _ _|   \ | 
   _ \   |  | |\/ |   |   .  |    |     (   | (_ |   |   .  | 
 _/  _\ ___/ _|  _| ___| _|\_|   ____| \___/ \___| ___| _|\_| 
                                                              ");
            Console.ResetColor();

            #endregion
            Console.WriteLine();

            Console.Write("UserName: ");
            string adminname = Console.ReadLine()!;

            if (adminname != "Admin")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Wrong UserName!");
                Console.ResetColor();
                Console.ReadKey();
                continue;
            }

            Console.WriteLine();

            Console.Write("Password:");
            string pss = Console.ReadLine()!;

            Console.WriteLine();

            string adminpss = "kaisumwong13";

            if (pss != adminpss)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Wrong Password!");
                Console.ResetColor();
                Console.ReadKey();
                continue;
            }



            Console.Clear();
            Console.Write("Loading");
            for (int i = 0; i < 10; i++)
            {
                Console.Write(".");
                Thread.Sleep(100); // pause for 200 milliseconds
            }
            //await Task.Delay(100);

            //Done Checking Here
            //Now Start Menu
            #region Admin Menu

            adminswitch();

            #endregion

            #endregion

            Console.ReadKey();

            break;

        case "2":

            #region Login/Checking (User)

            #region User Card Number

            Console.Clear();
            #region Art

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(@" __  __     ______     ______     ______        __         ______     ______     __     __   __    
/\ \/\ \   /\  ___\   /\  ___\   /\  == \      /\ \       /\  __ \   /\  ___\   /\ \   /\ ""-.\ \   
\ \ \_\ \  \ \___  \  \ \  __\   \ \  __<      \ \ \____  \ \ \/\ \  \ \ \__ \  \ \ \  \ \ \-.  \  
 \ \_____\  \/\_____\  \ \_____\  \ \_\ \_\     \ \_____\  \ \_____\  \ \_____\  \ \_\  \ \_\\""\_\ 
  \/_____/   \/_____/   \/_____/   \/_/ /_/      \/_____/   \/_____/   \/_____/   \/_/   \/_/ \/_/ 
                                                                                                   ");
            Console.ResetColor();

            #endregion
            Console.WriteLine("Card Number");
            string cardnum = Console.ReadLine()!;

            var finduser = db.customers.FirstOrDefault(x => x.CardNumber == cardnum);

            if (finduser == null || finduser.Status == "Deleted")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Card Number Not Exist!");
                Console.ResetColor();
                Console.ReadKey();
                continue;
            }

            #endregion

            Console.WriteLine();

            #region User Password

            Console.WriteLine("Password");
            string cardnumpass = Console.ReadLine()!;

            var finduserpass = db.customers.FirstOrDefault(x => x.CardNumber == cardnum && x.Password == cardnumpass);

            if (finduser.IsLockedOut)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Your account has been locked due to too many failed attempts. Please contact customer support.");
                Console.ReadKey();
                Console.ResetColor();
                continue;
            }

            if (finduserpass == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Wrong Password! You Have 3 Attempts " + finduser.LoginTry);
                Console.ReadKey();
                Console.ResetColor();

                finduser.LoginTry--;

                if (finduser.LoginTry <= 0)
                {
                    string MSG = "";
                    SendSMS(MSG);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You Have Exceeded The Number Of Allowed Login Attempts. Please Try Again Later.");
                    Console.ReadKey();
                    Console.ResetColor();
                    finduser.IsLockedOut = true;
                    finduser.Status = "Locked";
                    db.SaveChanges();
                }

                continue;
            }

            #endregion

            Console.WriteLine();

            #region User Menu

            Console.Clear();

            userswitch(finduserpass);

            #endregion

            #endregion

            break;

        case "3":

            Environment.Exit(0);

            break;
    }
}

void adminswitch()
{
    bool adminmenu2 = true;

    while (adminmenu2)
    {
        Console.Clear();
        #region Art
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(@"__/\\\\____________/\\\\__/\\\\\\\\\\\\\\\__/\\\\\_____/\\\__/\\\________/\\\_        
 _\/\\\\\\________/\\\\\\_\/\\\///////////__\/\\\\\\___\/\\\_\/\\\_______\/\\\_       
  _\/\\\//\\\____/\\\//\\\_\/\\\_____________\/\\\/\\\__\/\\\_\/\\\_______\/\\\_      
   _\/\\\\///\\\/\\\/_\/\\\_\/\\\\\\\\\\\_____\/\\\//\\\_\/\\\_\/\\\_______\/\\\_     
    _\/\\\__\///\\\/___\/\\\_\/\\\///////______\/\\\\//\\\\/\\\_\/\\\_______\/\\\_    
     _\/\\\____\///_____\/\\\_\/\\\_____________\/\\\_\//\\\/\\\_\/\\\_______\/\\\_   
      _\/\\\_____________\/\\\_\/\\\_____________\/\\\__\//\\\\\\_\//\\\______/\\\__  
       _\/\\\_____________\/\\\_\/\\\\\\\\\\\\\\\_\/\\\___\//\\\\\__\///\\\\\\\\\/___ 
        _\///______________\///__\///////////////__\///_____\/////_____\/////////_____");
        Console.ResetColor();

        #endregion

        Console.WriteLine();

        Console.WriteLine("1.Add Bank Account");
        Console.WriteLine("2.Manage Bank Acount");
        Console.WriteLine("3.Logout");
        Console.WriteLine("Enter Your Option");

        string adminmenu = Console.ReadLine()!;

        switch (adminmenu)
        {
            case "1":

                #region Name

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("Add Bank Account");
                Console.WriteLine("------------------");
                Console.ResetColor();
                Customer newcustomer = new Customer();

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Enter Full Name");
                Console.ResetColor();
                newcustomer.FullName = Console.ReadLine()!.Trim();

                Regex regexname = new Regex("^[a-zA-Z ]+$");

                if (!regexname.IsMatch(newcustomer.FullName))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Name Cannot Contains Numberic!");
                    Console.ReadKey();
                    Console.ResetColor();
                    continue;
                }

                #endregion

                Console.WriteLine();

                #region NRIC


                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Enter NRIC");
                Console.ResetColor();
                newcustomer.NRIC = getNRIC();

                if (!Regex.IsMatch(newcustomer.NRIC, @"(([[0-9]{2})(0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01]))-([0-9]{2})-([0-9]{4})"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine("Incorrect NRIC Format!");
                    Console.ReadKey();
                    Console.ResetColor();
                    continue;
                }

                //else
                //{
                //    break;
                //}


                #endregion

                Console.WriteLine();

                #region Bank Starting Balance
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Enter Bank Acount Starting Balance: ");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Minimum Starting Balance is RM250");
                Console.ResetColor();

                int startbalance = 0;
                try
                {
                    startbalance = Convert.ToInt32(Console.ReadLine());
                }

                catch
                {

                }

                if (startbalance < 250)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Minimum Starting Balance is RM250!");
                    Console.ResetColor();
                    Console.ReadKey();
                    continue;
                }

                else
                {
                    newcustomer.AccountBalance += startbalance;
                    Console.WriteLine("Your Have Deposit " + newcustomer.AccountBalance + " To Your Account");
                }

                #endregion

                Console.WriteLine();

                #region Generating Card/Card Pin

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Generating Card Details");
                Console.ResetColor();
                for (int i = 0; i < 10; i++)
                {
                    Console.Write(".");
                    Thread.Sleep(200); // pause for 200 milliseconds
                }

                Task.Delay(200);

                Random random = new Random();
                string creditCard = "";

                for (int i = 0; i < 12; i++)
                {
                    creditCard += random.Next(0, 10);

                    if ((i + 1) % 4 == 0 && i != 11)
                    {
                        creditCard += " ";
                    }
                }

                Random Accountnumber = new Random();
                string accountnumber = "";
                for (int p = 0; p < 7; p++)
                {
                    accountnumber += Accountnumber.Next(0, 10);
                }

                Random Accountpass = new Random();
                string accountpass = "";
                for (int p = 0; p < 6; p++)
                {
                    accountpass += Accountpass.Next(0, 10);
                }

                Console.Clear();

                int birthYear = int.Parse(newcustomer.NRIC.Substring(0, 2));
                int birthMonth = int.Parse(newcustomer.NRIC.Substring(2, 2));
                int birthDay = int.Parse(newcustomer.NRIC.Substring(4, 2));

                int sexDigit = int.Parse(newcustomer.NRIC.Substring(11, 1));
                string sex = (sexDigit % 2 == 0) ? "Female" : "Male";
                newcustomer.Gender = sex;

                int century = GetCentury(newcustomer.NRIC[0]);
                DateTime birthdate = new DateTime(century + birthYear, birthMonth, birthDay);

                Console.WriteLine("User Name: " + newcustomer.FullName);
                Console.WriteLine("User BirthDate: " + birthdate.ToShortDateString()!);
                Console.WriteLine("Gender: " + sex);
                Console.WriteLine("------------------------------");
                Thread.Sleep(300);

                Console.WriteLine();

                Console.WriteLine("Credit Card Number: " + creditCard);
                newcustomer.CardNumber = creditCard;
                Thread.Sleep(300);

                Console.WriteLine();

                Console.WriteLine("Account Number: " + accountnumber);
                newcustomer.AccountNumber = accountnumber;
                Thread.Sleep(300);

                Console.WriteLine();

                Console.WriteLine("Account Password: " + accountpass);
                newcustomer.Password = accountpass;
                Thread.Sleep(300);

                newcustomer.Status = "Active";

                db.customers.Add(newcustomer);
                db.SaveChanges();

                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("New Bank Account Successfully Added");
                Console.ResetColor();
                Thread.Sleep(300);

                //break;

                #endregion

                Console.ReadKey();

                break;

            case "2":

                #region Manage Bank Account

                Console.Clear();
                var customertable = from c in db.customers
                                    select c;

                ConsoleTable customer = new ConsoleTable("ID", "Account Number", "Full Name", "Status");

                foreach (var p in customertable)
                {
                    customer.AddRow(p.CustomerId, p.AccountNumber, p.FullName, p.Status);
                }

                customer.Write();

                Console.WriteLine();

                Console.Write("Enter Account ID To Edit:");
                int ID = 0;
                try
                {
                    ID = Convert.ToInt32(Console.ReadLine())!;
                }

                catch
                {

                }

                Console.WriteLine();
                Console.WriteLine("-------------------------");

                var result = (from c in db.customers
                              where c.CustomerId == ID
                              select c).FirstOrDefault();

                if (result == null)
                {
                    Console.WriteLine("No User Data");
                }

                else
                {
                    Console.WriteLine("FullName: " + result.FullName);
                    Console.WriteLine("Gender: " + result.Gender);
                    Console.WriteLine("NRIC: " + result.NRIC);
                    Console.WriteLine("Bank Account Number: " + result.AccountNumber);
                    Console.WriteLine("Bank Account Balance: " + result.AccountBalance);
                    Console.WriteLine("ATM Card Number: " + result.CardNumber);
                    Console.WriteLine("ATM Pin Number: " + result.Password);
                    Console.WriteLine("Status: " + result.Status);

                    Console.WriteLine();

                    Console.WriteLine("Edit (e) or Delete (d)? ");
                    ConsoleKeyInfo key1 = Console.ReadKey();

                    if (key1.Key == ConsoleKey.E)
                    {

                        Console.WriteLine();
                        Console.WriteLine();

                        Console.WriteLine("1.Name Edit");
                        Console.WriteLine("2.Status Change");
                        Console.WriteLine("3.Exit");
                        string bankaccount = Console.ReadLine()!;

                        switch (bankaccount)
                        {
                            case "1":

                                #region Name Change
                                Console.Write("Type In Name to Change:");
                                result.FullName = Console.ReadLine()!;
                                Console.WriteLine("Changes Save");
                                db.SaveChanges();

                                #endregion

                                break;

                            case "2":

                                #region Status Change

                                if (result.Status == "Active")
                                {
                                    Console.WriteLine();

                                    Console.WriteLine("This Account Current Status: " + result.Status);
                                    Console.WriteLine("1.Delete Account | 2.Exit");
                                    string opt3 = Console.ReadLine()!;

                                    switch (opt3)
                                    {
                                        case "1":

                                            #region Active Account to Delete

                                            Console.WriteLine("Are You Sure? Yes (y) or No (n)?");
                                            ConsoleKeyInfo key3 = Console.ReadKey();

                                            Console.WriteLine();

                                            if (key3.Key == ConsoleKey.Y)
                                            {
                                                result.Status = "Deleted";
                                                Console.WriteLine("Selected bank Account Successfully Deleted");
                                                Console.ReadKey();
                                                db.SaveChanges();
                                            }

                                            else if (key3.Key == ConsoleKey.N)
                                            {
                                                break;
                                            }

                                            else
                                            {
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.WriteLine("Please Choose An Option!");
                                                Console.ResetColor();
                                                Console.ReadKey();
                                                continue;
                                            }

                                            #endregion

                                            break;

                                        case "2":

                                            break;
                                    }

                                }

                                if (result.Status == "Deleted" || result.Status == "Locked")
                                {
                                    Console.WriteLine("This Account Current Status: " + result.Status);
                                    Console.WriteLine("Do you Want to Unlocked Account? Yes (y) or No (n)");
                                    ConsoleKeyInfo key4 = Console.ReadKey();

                                    Console.WriteLine();

                                    if (key4.Key == ConsoleKey.Y)
                                    {
                                        result.Status = "Active";
                                        result.IsLockedOut = false;
                                        result.LoginTry = 3;
                                        Console.WriteLine("This Account Has Been: " + result.Status);
                                        Console.ReadKey();
                                        db.SaveChanges();
                                        break;
                                    }

                                    else if (key4.Key == ConsoleKey.N)
                                    {
                                        break;
                                    }
                                }

                                #endregion

                                break;


                            case "3":

                                #region Exit Page

                                #endregion

                                break;
                        }

                    }

                    else if (key1.Key == ConsoleKey.D)
                    {
                        Console.WriteLine();

                        Console.WriteLine("Are You Sure? Yes (y) or No (n)?");
                        ConsoleKeyInfo key3 = Console.ReadKey();

                        Console.WriteLine();

                        if (key3.Key == ConsoleKey.Y)
                        {
                            result.Status = "Deleted";
                            Console.WriteLine("Selected bank Account Successfully Deleted");
                            Console.ReadKey();
                            db.SaveChanges();
                        }

                        else if (key3.Key == ConsoleKey.N)
                        {
                            break;
                        }

                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Please Choose An Option!");
                            Console.ResetColor();
                            Console.ReadKey();
                            continue;
                        }
                    }
                }


                Console.ReadKey();

                #endregion

                break;

            case "3":

                #region Logout

                Console.WriteLine("Do you Want to Logout? Yes (y) or No (n)?");
                ConsoleKeyInfo logoutkey = Console.ReadKey();

                if (logoutkey.Key == ConsoleKey.Y)
                {

                    adminmenu2 = false;

                }

                else if (logoutkey.Key == ConsoleKey.N)
                {
                    break;
                }

                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please Select An Option!");
                    Console.ResetColor();
                }

                #endregion

                break;
        }
    }
}

void userswitch(Customer finduser)
{
    bool inusermenu = true;

    while (inusermenu)
    {
        Console.Clear();
        #region Art
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(@"  __  ___             
  /  |/  /__ ___  __ __
 / /|_/ / -_) _ \/ // /
/_/  /_/\__/_//_/\_,_/ 
                       ");
        Console.ResetColor();

        #endregion
        Console.WriteLine("1.Check Balance");
        Console.WriteLine("2.Deposit");
        Console.WriteLine("3.Withdraw");
        Console.WriteLine("4.Third Party Transfer");
        Console.WriteLine("5.Transaction History");
        Console.WriteLine("6.Account Setting");
        Console.WriteLine("7.Logout");
        Console.WriteLine("Enter An Option");
        string useropt = Console.ReadLine()!;

        switch (useropt)
        {
            case "1":

                Console.Clear();
                #region Art
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(@" $$$$$$$\          $$\                                     
$$  __$$\         $$ |                                    
$$ |  $$ |$$$$$$\ $$ |$$$$$$\ $$$$$$$\  $$$$$$$\ $$$$$$\  
$$$$$$$\ |\____$$\$$ |\____$$\$$  __$$\$$  _____$$  __$$\ 
$$  __$$\ $$$$$$$ $$ |$$$$$$$ $$ |  $$ $$ /     $$$$$$$$ |
$$ |  $$ $$  __$$ $$ $$  __$$ $$ |  $$ $$ |     $$   ____|
$$$$$$$  \$$$$$$$ $$ \$$$$$$$ $$ |  $$ \$$$$$$$\\$$$$$$$\ 
\_______/ \_______\__|\_______\__|  \__|\_______|\_______|");

                Console.ResetColor();
                Console.WriteLine();

                #endregion
                Console.WriteLine("Your Current Balance: " + finduser.AccountBalance);
                Console.ReadKey();

                break;

            case "2":

                #region Deposit

                bool whiledeposit = true;

                while (whiledeposit)
                {
                    Console.Clear();
                    Console.WriteLine("Enter The Amount You Want to Deposit");

                    int userdeposit = 0;

                    try
                    {
                        userdeposit = Convert.ToInt32(Console.ReadLine())!;
                    }

                    catch
                    {

                    }

                    //if (userdeposit <= 0)
                    //{
                    //    Console.ForegroundColor = ConsoleColor.Red;
                    //    Console.WriteLine("Please Enter A Valid Postive Amount");
                    //    Console.ResetColor();
                    //    Console.ReadKey();
                    //}

                    if (userdeposit < 10 || userdeposit % 10 != 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid deposit amount. Only RM10, RM20, RM50, and RM100 banknotes are accepted.");
                        Console.ResetColor();
                        Console.ReadKey();
                    }

                    if (userdeposit > 100 && userdeposit % 100 == 0)
                    {
                        // Add the deposit to the account balance
                        finduser.AccountBalance += userdeposit;
                        Console.WriteLine("Deposit of RM{0} was successful. Your new balance is RM{1}.", userdeposit, finduser.AccountBalance);

                        Transaction transhis = new Transaction();
                        transhis.Account_number = finduser.AccountNumber;
                        transhis.UserId = finduser.CustomerId;
                        transhis.Credit = userdeposit;
                        transhis.Debit = 0;
                        transhis.CreateDate = DateTime.Now;
                        transhis.Total = userdeposit;
                        transhis.Description = ("Debit");
                        db.transactions.Add(transhis);

                        db.SaveChanges();
                        Console.ReadKey();
                        whiledeposit = false;
                    }


                    else
                    {
                        switch (userdeposit)
                        {
                            case 10:
                            case 20:
                            case 50:
                            case 100:

                                // Add the deposit to the account balance
                                finduser.AccountBalance += userdeposit;
                                Console.WriteLine("Deposit of RM{0} was successful. Your new balance is RM{1}.", userdeposit, finduser.AccountBalance);

                                Transaction transhis = new Transaction();
                                transhis.Account_number = finduser.AccountNumber;
                                transhis.UserId = finduser.CustomerId;
                                transhis.Credit = userdeposit;
                                transhis.Debit = 0; ;
                                transhis.CreateDate = DateTime.Now;
                                transhis.Total = userdeposit;
                                transhis.Description = ("Debit");
                                db.transactions.Add(transhis);

                                Console.ReadKey();
                                whiledeposit = false;
                                break;

                            default:

                                break;
                        }
                        db.SaveChanges();
                    }
                }
                #endregion

                break;

            case "3":

                #region Withdraw/Fast Cash

                Console.Clear();

                #region Art
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(@"██╗    ██╗██╗████████╗██╗  ██╗██████╗ ██████╗  █████╗ ██╗    ██╗
██║    ██║██║╚══██╔══╝██║  ██║██╔══██╗██╔══██╗██╔══██╗██║    ██║
██║ █╗ ██║██║   ██║   ███████║██║  ██║██████╔╝███████║██║ █╗ ██║
██║███╗██║██║   ██║   ██╔══██║██║  ██║██╔══██╗██╔══██║██║███╗██║
╚███╔███╔╝██║   ██║   ██║  ██║██████╔╝██║  ██║██║  ██║╚███╔███╔╝
 ╚══╝╚══╝ ╚═╝   ╚═╝   ╚═╝  ╚═╝╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═╝ ╚══╝╚══╝ ");

                Console.ResetColor();
                #endregion

                Console.WriteLine("1.Withdraw Amount");
                Console.WriteLine("2.Fast Cash");
                string wfopt = Console.ReadLine()!;

                switch (wfopt)
                {
                    #region(Case 1) WithDraw Amount

                    case "1":

                        Console.Clear();
                        Console.WriteLine("Type in the amount you want to Withdraw");
                        int takemoney = 0;
                        try
                        {
                            takemoney = Convert.ToInt32(Console.ReadLine());
                        }

                        catch
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Please Enter a Valid Positive Amount");
                            Console.ResetColor();
                            Console.ReadKey();
                        }

                        if (finduser.AccountBalance - takemoney < 250)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Account Minimum Balance Must Have RM250");
                            Console.ResetColor();
                            Console.ReadKey();
                            break;
                        }

                        if (takemoney > finduser.AccountBalance)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Insufficient Balance to Withdraw the Amount");
                            Console.ResetColor();
                            Console.ReadKey();
                            break;
                        }

                        if (takemoney > 0)
                        {
                            Console.WriteLine("Withdraw Success, You Withdraw " + "RM" + takemoney);
                            finduser.AccountBalance -= takemoney;

                            Transaction transhis = new Transaction();
                            transhis.Account_number = finduser.AccountNumber;
                            transhis.UserId = finduser.CustomerId;
                            transhis.Credit = 0;
                            transhis.Debit = takemoney;
                            transhis.CreateDate = DateTime.Now;
                            transhis.Total = takemoney;
                            transhis.Description = ("WithDraw");
                            db.transactions.Add(transhis);

                            Console.WriteLine();

                            Console.WriteLine("Your Current Account Balance: " + finduser.AccountBalance);
                            Console.ReadKey();
                            db.SaveChanges();
                        }

                        break;

                    #endregion

                    #region (Case 2) Fast Cash

                    case "2":

                        FASTCASH(finduser);

                        break;

                        #endregion
                }

                #endregion

                break;

            case "4":

                #region Third Party Transfer

                Console.Clear();
                Console.WriteLine("Third Party Transfer");

                Console.WriteLine();

                Console.WriteLine("Recipient's Bank Account Number");
                string recipentacc = Console.ReadLine()!;

                var receipt = db.customers.FirstOrDefault(x => x.AccountNumber == recipentacc);

                Console.WriteLine();

                if (receipt == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("This Bank Account Not Exist!");
                    Console.ReadKey();
                    break;
                }

                if (receipt.Status == "Deleted")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("This Bank Account Not Exist");
                    Console.ReadKey();
                    break;
                }

                if (recipentacc == finduser.AccountNumber)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Cannot Transfer to Own Account!");
                    Console.ResetColor();
                    Console.ReadKey();
                }

                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Recipent's Account Name: " + receipt.FullName);
                    Console.WriteLine("Do you Want to Transfer to this Account? Yes(Y) or No(N)");
                    Console.ResetColor();
                    string yesorno = Console.ReadLine()!;

                    Console.WriteLine();

                    if (yesorno.ToUpper() == "Y")
                    {
                        Console.WriteLine("Enter The Amount You Want to Transfer");
                        decimal transamount = 0;

                        try
                        {
                            transamount = Convert.ToDecimal(Console.ReadLine());
                        }

                        catch
                        {

                        }

                        if (finduser.AccountBalance - transamount < 250)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Account Minimum Balance Must Have RM250");
                            Console.ResetColor();
                            Console.ReadKey();
                            break;
                        }

                        if (transamount > 0)
                        {
                            var checkdailyspend = db.transactions.Where(x => x.CreateDate.Date == DateTime.Now.Date && x.Debit > 0);

                            if (checkdailyspend.Count() == 0)
                            {
                                finduser.DailySpend = 0;
                                db.SaveChanges();
                            }

                            var checktoday = db.transactions.Where(x => x.CreateDate.Date == DateTime.Now.Date).Sum(x => x.Debit);

                            if (finduser.DailySpend + transamount > 2000 || checktoday + transamount > 2000)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkBlue;
                                Console.WriteLine("Your Daily Spending Limit Is RM2000");
                                Console.ReadKey();
                                Console.ResetColor();
                                db.SaveChanges();
                                break;
                            }

                            Console.WriteLine("You Transfer " + "RM" + transamount + " for this Account");
                            finduser.AccountBalance -= transamount;
                            finduser.DailySpend += transamount;
                            receipt.AccountBalance += transamount;

                            #region User Transfer
                            Transaction thirdtrans = new Transaction();
                            thirdtrans.Account_number = finduser.AccountNumber;
                            thirdtrans.UserId = finduser.CustomerId;
                            thirdtrans.Credit = 0;
                            thirdtrans.Debit = transamount;
                            thirdtrans.CreateDate = DateTime.Now;
                            thirdtrans.Total = finduser.AccountBalance;
                            thirdtrans.Description = ("Transfer to " + receipt.AccountNumber);
                            db.transactions.Add(thirdtrans);
                            #endregion

                            #region Recipent Account 
                            Transaction thirdtransreceive = new Transaction();
                            thirdtransreceive.Account_number = receipt.AccountNumber;
                            thirdtransreceive.UserId = receipt.CustomerId;
                            thirdtransreceive.Credit = transamount;
                            thirdtransreceive.Debit = 0;
                            thirdtransreceive.CreateDate = DateTime.Now;
                            thirdtransreceive.Total = receipt.AccountBalance;
                            thirdtransreceive.Description = ("Received From " + finduser.AccountNumber);
                            db.transactions.Add(thirdtransreceive);

                            Console.ReadKey();
                            db.SaveChanges();
                            #endregion
                        }

                        if (transamount <= 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Please Enter A Valid Positive Amount ");
                            Console.ResetColor();
                            Console.ReadKey();
                        }
                    }

                    else if (yesorno.ToUpper() == "N")
                    {
                        break;
                    }

                }

                #endregion

                break;

            case "5":

                #region Transaction History

                Console.Clear();
                ConsoleTable transhistory = new ConsoleTable("Number", "Id", "Account Number", "Description", "Debit", "Credit", "Date and Time");
                int id = 0;
                foreach (var t in db.transactions.Where(x => x.UserId == finduser.CustomerId))
                {
                    id++;
                    Console.Clear();
                    transhistory.AddRow(id, t.Id, t.Account_number, t.Description, t.Debit, t.Credit, t.CreateDate);
                }

                transhistory.Write();

                Console.WriteLine("1.Show Recent 5 Transaction History");
                Console.WriteLine("2.Show Recent 10 Transaction History");
                string opttrans = Console.ReadLine()!;

                switch (opttrans)
                {
                    case "1":

                        Console.Clear();
                        ConsoleTable transhistorytop5 = new ConsoleTable("Id", "Account Number", "Description", "Debit", "Credit", "Date and Time");

                        var top5trans = (from t in db.transactions
                                         orderby t.CreateDate descending
                                         select t).Take(5);

                        foreach (var k in top5trans)
                        {
                            Console.Clear();
                            transhistorytop5.AddRow(k.Id, k.Account_number, k.Description, k.Debit, k.Credit, k.CreateDate);
                        }

                        transhistorytop5.Write();

                        Console.ReadKey();

                        break;


                    case "2":
                        Console.Clear();
                        ConsoleTable transhistorytop10 = new ConsoleTable("Id", "Account Number", "Description", "Debit", "Credit", "Date and Time");

                        var top10trans = (from t in db.transactions
                                          orderby t.CreateDate descending
                                          select t).Take(10);

                        foreach (var k in top10trans)
                        {
                            Console.Clear();
                            transhistorytop10.AddRow(k.Id, k.Account_number, k.Description, k.Debit, k.Credit, k.CreateDate);
                        }

                        transhistorytop10.Write();

                        Console.ReadKey();

                        break;
                }


                Console.ReadKey();

                #endregion

                break;

            case "6":

                #region User bank Account Detail

                Console.Clear();

                ConsoleTable customerdetail = new ConsoleTable("Name","Gender", "NRIC", "Password", "Account Balance", "Account Number", "Card Number");

                foreach (var c in db.customers.Where(x => x.CustomerId == finduser.CustomerId))
                {
                    customerdetail.AddRow(c.FullName,c.Gender, c.NRIC, c.Password, c.AccountBalance, c.AccountNumber, c.CardNumber);
                }

                customerdetail.Write();

                Console.WriteLine();

                #region moneycount

                decimal accountbalance = finduser.AccountBalance;
                int rm100Count = 0;
                int rm20Count = 0;
                int rm10Count = 0;
                int rm5Count = 0;
                int rm1Count = 0;

                while (accountbalance >= 100)
                {
                    rm100Count++;
                    accountbalance -= 100;
                }

                while (accountbalance >= 20)
                {
                    rm20Count++;
                    accountbalance -= 20;
                }

                while (accountbalance >= 10)
                {
                    rm10Count++;
                    accountbalance -= 10;
                }

                while (accountbalance >= 5)
                {
                    rm5Count++;
                    accountbalance -= 5;
                }

                while (accountbalance >= 1)
                {
                    rm1Count++;
                    accountbalance -= 1;
                }


                Console.WriteLine("=============================");
                Console.WriteLine($"You have {rm100Count} RM100 ");
                Console.WriteLine($"You have {rm20Count} RM20 ");
                Console.WriteLine($"You have {rm10Count} RM10 ");
                Console.WriteLine($"You have {rm5Count} RM5 ");
                Console.WriteLine($"You have {rm1Count} RM1 ");
                Console.WriteLine("=============================");


                #endregion

                Console.WriteLine();

                Console.WriteLine("1.Change Password | 2.Exit");
                string adopt = Console.ReadLine()!;

                if (adopt == "1")
                {
                    Console.WriteLine("Enter Old Password");
                    string oldpss = Console.ReadLine()!;

                    if (oldpss != finduser.Password)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Wrong Old Password!");
                        Console.ReadKey();
                        break;
                    }

                    Console.WriteLine("Enter a New Password");
                    string newpss1 = Console.ReadLine()!;

                    Regex regex1 = new Regex("^[0-9]{6}$"); // Regular expression to match only digits 0-9
                    bool isValid = regex1.IsMatch(newpss1);

                    if (isValid)
                    {

                    }

                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid input: " + newpss1 + " (input must only contain digits 0-9 and 6 digits)");
                        Console.ReadKey();
                        Console.ResetColor();
                        continue;
                    }

                    Console.WriteLine("Confirm New Password");
                    string newpss2 = Console.ReadLine()!;

                    if (newpss1 == newpss2)
                    {
                        Console.WriteLine("Password Change Successfully");
                        finduser.Password = newpss1;
                        db.SaveChanges();
                    }

                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Mismatch Password!");
                        Console.ReadKey();
                        break;
                    }
                }


                if (adopt == "2")
                {
                    break;
                }

                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please Choose an Option!");
                    Console.ReadKey();
                }

                #endregion

                break;

            case "7":

                inusermenu = false;

                break;
        }
    }
}

void FASTCASH(Customer finduser)
{
    Console.Clear();
    Console.WriteLine("Chosose An Option");
    Console.WriteLine("1.RM100");
    Console.WriteLine("2.RM200");
    Console.WriteLine("3.RM500");
    string fastcashopt = Console.ReadLine()!;

    #region Fast Cash Switch

    switch (fastcashopt)
    {
        case "1":
            Console.Clear();
            Console.WriteLine("You Have Taken RM100 Fast Cash");
            finduser.AccountBalance -= 100;

            Transaction transhis = new Transaction();
            transhis.Account_number = finduser.AccountNumber;
            transhis.UserId = finduser.CustomerId;
            transhis.Credit = 0;
            transhis.Debit = 100;
            transhis.CreateDate = DateTime.Now;
            transhis.Total = finduser.AccountBalance;
            transhis.Description = ("FastCash RM100");
            db.transactions.Add(transhis);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Your Current Balance " + finduser.AccountBalance);
            Console.WriteLine("Money,money,money!");
            Console.ResetColor();
            Console.ReadKey();
            db.SaveChanges();
            break;

        case "2":
            Console.Clear();
            Console.WriteLine("You Have Taken RM200 Fast Cash");
            finduser.AccountBalance -= 200;

            Transaction transhis2 = new Transaction();
            transhis2.Account_number = finduser.AccountNumber;
            transhis2.UserId = finduser.CustomerId;
            transhis2.Credit = 0;
            transhis2.Debit = 200;
            transhis2.CreateDate = DateTime.Now;
            transhis2.Total = finduser.AccountBalance;
            transhis2.Description = ("FastCash RM200");
            db.transactions.Add(transhis2);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Your Current Balance " + finduser.AccountBalance);
            Console.WriteLine("Money,money,money!");
            Console.ResetColor();
            Console.ReadKey();
            db.SaveChanges();
            break;


        case "3":
            Console.Clear();
            Console.WriteLine("You Have Taken RM500 Fast Cash");
            finduser.AccountBalance -= 500;

            Transaction transhis5 = new Transaction();
            transhis5.Account_number = finduser.AccountNumber;
            transhis5.UserId = finduser.CustomerId;
            transhis5.Credit = 0;
            transhis5.Debit = 500;
            transhis5.CreateDate = DateTime.Now;
            transhis5.Total = finduser.AccountBalance;
            transhis5.Description = ("FastCash RM500");
            db.transactions.Add(transhis5);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Your Current Balance " + finduser.AccountBalance);
            Console.WriteLine("Money,money,money!");
            Console.ResetColor();
            Console.ReadKey();
            db.SaveChanges();
            break;
    }

    #endregion
}

void SendSMS(string MSG)
{
    string accountSid = "AC583a1716e545ac6393d07883fac3f75e";
    string authToken = "8a8ab1d4507c6f6028fc19bd9d4b7783";

    TwilioClient.Init(accountSid, authToken);

    var message = MessageResource.Create(
        body: "Your account has been locked due to too many failed attempts. Please contact customer support.",
        from: new Twilio.Types.PhoneNumber("++15855600552"),
        to: new Twilio.Types.PhoneNumber("+60108786893")//自己电话号码
    );

}

static int GetCentury(char firstDigit)
{
    switch (firstDigit)
    {
        case '0':
            return 2000;
        case '1':
            return 2010;
        case '2':
            return 2020;
        case '5':
            return 1950;
        case '6':
            return 1960;
        case '7':
            return 1970;
        case '8':
            return 1980;
        case '9':
            return 1990;
        default:
            throw new ArgumentException("Invalid NRIC first digit.");

    }
}

static string getNRIC()
{
    string NumStr = ""; ConsoleKeyInfo getNum;
    while (true)
    {
        getNum = Console.ReadKey(true);
        if (NumStr.Length == 14)
        {
            if (getNum.Key != ConsoleKey.Backspace && getNum.Key != ConsoleKey.Enter)
                continue;
        }
        if (getNum.Key == ConsoleKey.Enter)
        {
            if (NumStr.Length == 0)
                continue;
            break;
        }
        else if (Char.IsNumber(getNum.KeyChar))
        {
            if (NumStr.Length == 6 || NumStr.Length == 9) { Console.Write("-"); NumStr += "-" + getNum.KeyChar.ToString(); }
            else { NumStr += getNum.KeyChar.ToString(); }
            Console.Write(getNum.KeyChar.ToString());
        }
        else if (getNum.Key == ConsoleKey.Backspace && NumStr.Length > 0)
        {
            if (NumStr.Length == 7 || NumStr.Length == 10) { NumStr = NumStr.Substring(0, NumStr.Length - 2); Console.Write("\b\b  \b\b"); } else { NumStr = NumStr.Substring(0, NumStr.Length - 1); Console.Write("\b \b"); }
            continue;
        }
    }
    return NumStr;
}
