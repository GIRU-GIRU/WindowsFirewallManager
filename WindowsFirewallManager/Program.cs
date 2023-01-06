using FirewallManager;


try
{
    PlatformUtils.Validate();

    ArgsProcessor argsProcessor = new(Environment.GetCommandLineArgs());
    argsProcessor.HandleInput();
}
catch (Exception ex)
{
    Console.WriteLine("Error - " + ex.GetBaseException().Message);
}









