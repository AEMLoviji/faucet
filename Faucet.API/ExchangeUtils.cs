namespace Faucet.API
{
    internal static class ExchangeUtils
    {
        public static decimal BtcToUsd(decimal btc, decimal rate)
        {
            return decimal.Round(btc * rate, 2);
        }
    }
}
