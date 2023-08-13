using Terraria.Localization;

namespace InfernumMode.Common.UtilityMethods
{
    public static class LangHelper
    {
        public static LocalizedText GetLocalization(string key)
        {
            return Language.GetOrRegister(InfernumMode.Instance.GetLocalizationKey(key));
        }
    }
}