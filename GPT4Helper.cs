using OpenAI_API;

class GPT4Helper
{
    private readonly OpenAIAPI _api;

    public GPT4Helper(string apiKey)
    {
        _api = new OpenAIAPI(apiKey);
    }

    public async Task<string> AskGPT4Async(string prompt, string contextCode)
    {
        var chat = _api.Chat.CreateConversation();
        chat.Model = "gpt-4";
        chat.AppendSystemMessage("Bạn là lập trình viên .NET, hãy viết mã chuẩn.");
        chat.AppendUserInput($"Code hiện tại:\n{contextCode}\n\nTôi muốn: {prompt}");
        return await chat.GetResponseFromChatbot();
    }
}
