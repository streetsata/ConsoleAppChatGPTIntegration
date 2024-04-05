using Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions;

namespace ConsoleAppChatGPTIntegration
{
    public interface IOpenAIProxy
    {
        Task<ChatCompletionMessage[]> SendChatMessage(string message);
        void SetSystemMessage(string systemMessage);
    }
}
