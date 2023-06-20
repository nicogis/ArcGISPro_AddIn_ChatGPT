using OpenAI_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAMChatGPT
{
    internal class Models
    {
        
        public static Model GPT3_5_0613 => new Model("gpt-3.5-turbo-0613") { OwnedBy = "openai" };

        public static Model GPT4_0613 => new Model("gpt-4-0613") { OwnedBy = "openai" };

        public static Model GPT4_32k_0613 => new Model("gpt-4-32k-0613") { OwnedBy = "openai" };

        public static Model GPT3_5_16k => new Model("gpt-3.5-turbo-16k") { OwnedBy = "openai" };

        public static Model GPT3_5_16k_0613 => new Model("gpt-3.5-turbo-16k-0613") { OwnedBy = "openai" };
    }
}
