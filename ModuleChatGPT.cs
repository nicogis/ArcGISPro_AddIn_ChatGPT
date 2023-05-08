using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;

namespace PAMChatGPT
{
    internal class ModuleChatGPT : Module
    {
        private static ModuleChatGPT _this = null;

        /// <summary>
        /// Retrieve the singleton instance to this module here
        /// </summary>
        public static ModuleChatGPT Current => _this ??= (ModuleChatGPT)FrameworkApplication.FindModule("PAMChatGPT_Module");

        public static Bot Bot => Bot.GetBot();

        #region Overrides
        /// <summary>
        /// Called by Framework when ArcGIS Pro is closing
        /// </summary>
        /// <returns>False to prevent Pro from closing, otherwise True</returns>
        protected override bool CanUnload()
        {
            //TODO - add your business logic
            //return false to ~cancel~ Application close
            return true;
        }

        #endregion Overrides

    }
}
