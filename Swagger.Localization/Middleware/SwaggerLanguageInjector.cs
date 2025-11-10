using System.Globalization;

namespace Swagger.Localization.Middleware;
public class SwaggerLanguageInjector
{
    private readonly IList<CultureInfo> _supportedCultures;

    public SwaggerLanguageInjector(IList<CultureInfo> supportedCultures)
    {
        _supportedCultures = supportedCultures;
    }

    public string InjectLanguageSelector(string html)
    {
        var languageOptions = _supportedCultures.Select(culture =>
            $@"<option value=""?lang={culture.Name}"">{culture.NativeName}</option>"
        ).Aggregate((a, b) => a + b);

        var languageSelector = $@"
            <div style='position: fixed; top: 20px; right: 20px; z-index: 10000;'>
                <select onchange='window.location.href = this.value'>
                    {languageOptions}
                </select>
            </div>
        ";

        return html.Replace("</body>", $"{languageSelector}</body>");
    }
}