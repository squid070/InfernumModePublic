using Terraria.Localization;

namespace InfernumMode.Common.UtilityMethods
{
    public static class LanguageUtilities
    {
        public static LocalizedText GetLocalization(string key)
        {
            return Language.GetOrRegister(InfernumMode.Instance.GetLocalizationKey(key));
        }
    }
}