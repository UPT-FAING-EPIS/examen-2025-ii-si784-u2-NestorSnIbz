using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Playwright;
using NUnit.Framework;

namespace UPTRepo.UiTests;

[TestFixture]
public class SearchTests
{
    private static string[] Terms => new[]
    {
        "web",
        "base de datos",
        "mobil",
        "inteligencia de negocios",
        "inteligencia artificial"
    };

    [Test]
    [TestCase("chromium")]
    [TestCase("webkit")]
    public async Task BusquedaTecnologia_DevuelveResultados_EnDosNavegadores(string browserName)
    {
        using var playwright = await Playwright.CreateAsync();
        IBrowser browser = await LaunchBrowserAsync(playwright, browserName);

        string videosDir = Path.Combine("artifacts", "videos", browserName);
        Directory.CreateDirectory(videosDir);

        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            RecordVideoDir = videosDir,
            ViewportSize = new ViewportSize { Width = 1280, Height = 800 }
        });

        foreach (var term in Terms)
        {
            var page = await context.NewPageAsync();

            await page.GotoAsync("https://repositorio.upt.edu.pe/");

            // Intento principal: un textbox por accesibilidad ARIA.
            var searchBox = page.GetByRole(AriaRole.Textbox);
            if (await searchBox.CountAsync() == 0)
            {
                // Fallbacks comunes en DSpace: name='query', id='search', placeholder 'Buscar'.
                searchBox = page.Locator("input[name='query'], input#search, input[placeholder*='Buscar']");
            }

            Assert.That(await searchBox.CountAsync(), Is.GreaterThan(0), "No se encontró el cuadro de búsqueda en el repositorio.");

            await searchBox.First.FillAsync(term);
            await page.Keyboard.PressAsync("Enter");

            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            // Heurística de resultados: enlaces a items suelen contener '/handle/'.
            // Para cumplir el criterio de aceptación CA1, exigimos al menos un resultado.
            await page.WaitForSelectorAsync("a[href*='/handle/']", new() { Timeout = 15000 });
            var itemLinks = page.Locator("a[href*='/handle/']");
            int resultsCount = await itemLinks.CountAsync();

            Assert.That(resultsCount, Is.GreaterThan(0),
                $"La búsqueda '{term}' no produjo resultados en el repositorio.");

            // Cerramos la página para finalizar y guardar el video automáticamente.
            await page.CloseAsync();
        }

        await context.CloseAsync();
        await browser.CloseAsync();
    }

    private static Task<IBrowser> LaunchBrowserAsync(IPlaywright playwright, string browserName)
    {
        return browserName switch
        {
            "chromium" => playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true }),
            "firefox" => playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true }),
            "webkit" => playwright.Webkit.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true }),
            _ => throw new ArgumentOutOfRangeException(nameof(browserName), browserName, "Navegador no soportado")
        };
    }
}