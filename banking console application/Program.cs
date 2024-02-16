using System;
using NLog;
using BANKING_APPLICATION;


class Program
{
    static void Main()
    {
        try
        {
            ATM_BANKING_CONSOLE_APPLICATION bankingApp = new ATM_BANKING_CONSOLE_APPLICATION();
            baratis_mflobelis_monacemebi validatedUser = ATM_BANKING_CONSOLE_APPLICATION.Validation();
            ATM_BANKING_CONSOLE_APPLICATION.Menu(validatedUser);
        }

        catch (Exception ex)
        {
            // get a Logger object and log exception here using NLog. 
            // this will use the "fileLogger" logger from NLog.config file
            Logger logger = LogManager.GetLogger("fileLogger");

            // add custom message and pass in the exception
            logger.Error(ex, $"Error: {ex.Message}");
        }
    }
}