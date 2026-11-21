using KompasKompetencji.Web.Assistant;

namespace KompasKompetencji.Tests.Assistant;

public class EvaluationLevelGuardTests
{
    [Theory]
    [InlineData("Jaki poziom dać Annie za współpracę?")]
    [InlineData("Jaką ocenę mam wystawić Kowalskiemu?")]
    [InlineData("Ile punktów przyznać za rzetelność?")]
    [InlineData("Czy mogę dać mu 9 punktów za komunikację?")]
    [InlineData("Oceń go za komunikację.")]
    [InlineData("Na jakim poziomie oceniłbyś jego współpracę?")]
    [InlineData("Jaką ocenę powinna dostać Anna?")]
    [InlineData("Powinienem postawić jej poziom powyżej oczekiwań?")]
    public void RecognizesRequestsToSetLevel(string question)
    {
        Assert.True(EvaluationLevelGuard.IsLevelRequest(question));
    }

    [Theory]
    [InlineData("Jaka jest skala ocen w ocenie okresowej?")]
    [InlineData("Co oznacza poziom powyżej oczekiwań?")]
    [InlineData("Kto wystawia ocenę okresową?")]
    [InlineData("Ile punktów ma skala w pierwszej ocenie?")]
    [InlineData("Kiedy pracownik otrzymuje ocenę negatywną?")]
    [InlineData("Jak ustala się ogólny poziom spełniania kryteriów?")]
    [InlineData("Ile dni ma pracownik na sprzeciw od oceny okresowej?")]
    [InlineData("")]
    public void IgnoresQuestionsAboutRules(string question)
    {
        Assert.False(EvaluationLevelGuard.IsLevelRequest(question));
    }
}
