namespace eth.Telegram.BotApi.Events
{
    public delegate void RequestEventHandler(object sender, RequestEventArgs args);
    public delegate void ResponseEventHandler(object sender, ResponseEventArgs args);
}
