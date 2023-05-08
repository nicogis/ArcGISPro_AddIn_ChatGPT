namespace PAMChatGPT.Properties
{
    internal sealed partial class Settings
    {

        public Settings()
        {
            if (UpgradeNeeded)
            {
                UpgradeNeeded = false;
                Save();
                Upgrade();
            }
        }
    }
}
