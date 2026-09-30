# Звіт з аналізу інкапсуляції в Open-Source проєкті

## Тема

**Аналіз інкапсуляції в open-source проєктах. Практики валідації полів.**

## Мета роботи

Дослідити, як принципи інкапсуляції та валідації даних реалізуються в реальному open-source проєкті на C#, а також проаналізувати різні підходи до перевірки даних.

## 1. Обраний проєкт

* **Назва:** ASP.NET Core
* **Мова програмування:** C#
* **Розробник:** .NET Foundation та спільнота
* **GitHub:** https://github.com/dotnet/aspnetcore
* **Ліцензія:** MIT

Для аналізу обрано open-source проєкт **ASP.NET Core**. Це великий проєкт на C#, який містить велику кількість класів, властивостей, методів та механізмів перевірки вхідних даних.

У роботі проаналізовано код із підсистеми ASP.NET Core Identity.

### Посилання на файли

1. `DataProtectorTokenProvider.cs`
   https://github.com/dotnet/aspnetcore/blob/main/src/Identity/Core/src/DataProtectorTokenProvider.cs

2. `IdentityApiEndpointRouteBuilderExtensions.cs`
   https://github.com/dotnet/aspnetcore/blob/main/src/Identity/Core/src/IdentityApiEndpointRouteBuilderExtensions.cs

## 2. Аналіз інкапсуляції

### Клас: `DataProtectorTokenProvider<TUser>`

**Посилання на клас:**

https://github.com/dotnet/aspnetcore/blob/main/src/Identity/Core/src/DataProtectorTokenProvider.cs

#### Опис класу

`DataProtectorTokenProvider<TUser>` використовується в ASP.NET Core Identity для створення та перевірки захищених токенів.

Клас працює з даними користувача та службами захисту даних.

#### Властивості

У класі використовуються властивості з обмеженим доступом:

```csharp
protected DataProtectionTokenProviderOptions Options { get; private set; }

protected IDataProtector Protector { get; private set; }
```

Властивості мають `protected` для читання та `private set` для зміни.

Це означає, що похідні класи можуть отримувати значення, але змінити їх безпосередньо не можуть.

Також використовується властивість тільки для читання:

```csharp
public string Name { get { return Options.Name; } }
```

Зовнішній код може отримати значення `Name`, але не може встановити його через `set`.

Ще один приклад:

```csharp
public ILogger<DataProtectorTokenProvider<TUser>> Logger { get; }
```

Це read-only властивість. Її значення встановлюється під час створення об'єкта.

#### Інкапсуляція

У цьому класі інкапсуляція забезпечується обмеженням доступу до внутрішніх даних.

Використання `private set` не дозволяє зовнішньому коду довільно змінювати стан властивостей.

Read-only властивості використовуються для надання зовнішньому коду доступу лише до необхідної інформації.

Таким чином, клас приховує частину своєї внутрішньої реалізації та контролює доступ до власного стану.

#### Валідація

У класі використовуються перевірки вхідних параметрів.

Наприклад:

```csharp
ArgumentNullException.ThrowIfNull(dataProtectionProvider);
```

Ця перевірка не дозволяє передати `null` замість обов'язкового об'єкта.

Також використовується перевірка користувача:

```csharp
ArgumentNullException.ThrowIfNull(user);
```

Такий підхід дозволяє виявити помилку одразу, ще до виконання основної логіки методу.

---

### Клас: `IdentityApiEndpointRouteBuilderExtensions`

**Посилання на клас:**

https://github.com/dotnet/aspnetcore/blob/main/src/Identity/Core/src/IdentityApiEndpointRouteBuilderExtensions.cs

#### Опис класу

`IdentityApiEndpointRouteBuilderExtensions` містить методи розширення, які використовуються для налаштування Identity API endpoints в ASP.NET Core.

Клас є `static`, тому він використовується для надання додаткової функціональності іншим компонентам.

#### Приватне поле

У класі використовується приватне поле:

```csharp
private static readonly EmailAddressAttribute _emailAddressAttribute = new();
```

Це поле:

* `private` — доступне тільки всередині класу;
* `static` — належить класу, а не окремому об'єкту;
* `readonly` — після ініціалізації його не можна переприсвоїти.

Це є прикладом інкапсуляції внутрішнього механізму перевірки email.

#### Приватна константа

Також використовується:

```csharp
private const int MaxPasskeyNameLength = 200;
```

Константа прихована від зовнішнього коду та використовується всередині класу для перевірки максимальної довжини назви passkey.

#### Перевірка аргументів

На початку окремих методів використовується:

```csharp
ArgumentNullException.ThrowIfNull(endpoints);
```

Таким способом перевіряється обов'язковий параметр.

Якщо значення `null`, метод одразу завершується винятком.

#### Валідація email

Для перевірки електронної пошти використовується `EmailAddressAttribute`:

```csharp
if (string.IsNullOrEmpty(email) || !_emailAddressAttribute.IsValid(email))
{
    return CreateValidationProblem(
        IdentityResult.Failed(
            userManager.ErrorDescriber.InvalidEmail(email)));
}
```

Спочатку перевіряється, чи рядок не є `null` або порожнім.

Після цього `EmailAddressAttribute` перевіряє формат електронної пошти.

Це приклад використання механізму `System.ComponentModel.DataAnnotations`.

#### Валідація довжини

Для назви passkey визначено максимальну довжину:

```csharp
private const int MaxPasskeyNameLength = 200;
```

Потім виконується перевірка:

```csharp
if (request.Name is { Length: > MaxPasskeyNameLength })
{
    return CreateValidationProblem(
        "InvalidPasskeyName",
        $"Passkey names must be no longer than {MaxPasskeyNameLength} characters.");
}
```

Якщо довжина назви перевищує 200 символів, формується повідомлення про помилку.

---

### Клас: `IdentityEndpointsConventionBuilder`

У файлі `IdentityApiEndpointRouteBuilderExtensions.cs` також визначено внутрішній клас:

```csharp
private sealed class IdentityEndpointsConventionBuilder
    (RouteGroupBuilder inner) : IEndpointConventionBuilder
```

#### Інкапсуляція

Клас оголошено як `private sealed`.

`private` означає, що він використовується тільки всередині відповідної частини реалізації.

`sealed` забороняє успадкування цього класу.

У класі є приватна властивість:

```csharp
private IEndpointConventionBuilder InnerAsConventionBuilder => inner;
```

Вона приховує внутрішній об'єкт `inner`.

Зовнішньому коду надаються тільки необхідні методи:

```csharp
public void Add(Action<EndpointBuilder> convention)
    => InnerAsConventionBuilder.Add(convention);

public void Finally(Action<EndpointBuilder> finallyConvention)
    => InnerAsConventionBuilder.Finally(finallyConvention);
```

Це демонструє принцип: внутрішня реалізація прихована, а зовні надається тільки необхідний інтерфейс роботи з об'єктом.

## 3. Практики валідації у властивостях та класах

### Приклад 1 — перевірка на `null`

У `DataProtectorTokenProvider<TUser>` використовується:

```csharp
ArgumentNullException.ThrowIfNull(dataProtectionProvider);
```

Цей механізм використовується для перевірки обов'язкових параметрів.

Перевага такого підходу полягає в тому, що помилка виявляється одразу.

### Приклад 2 — `ArgumentNullException`

Інший приклад:

```csharp
Logger = logger ?? throw new ArgumentNullException(nameof(logger));
```

Якщо `logger` має значення `null`, виникає `ArgumentNullException`.

Це захищає об'єкт від подальшої роботи з некоректною залежністю.

### Приклад 3 — `EmailAddressAttribute`

Для перевірки email використовується:

```csharp
private static readonly EmailAddressAttribute _emailAddressAttribute = new();
```

Перевірка виконується за допомогою:

```csharp
_emailAddressAttribute.IsValid(email)
```

Такий підхід зручний для перевірки даних, які повинні відповідати певному формату.

### Приклад 4 — перевірка довжини

Для назви passkey використовується обмеження:

```csharp
private const int MaxPasskeyNameLength = 200;
```

та умова:

```csharp
if (request.Name is { Length: > MaxPasskeyNameLength })
{
    return CreateValidationProblem(
        "InvalidPasskeyName",
        $"Passkey names must be no longer than {MaxPasskeyNameLength} characters.");
}
```

Це приклад перевірки конкретного правила предметної області.

### Висновки щодо валідації

В ASP.NET Core використовуються різні способи перевірки даних залежно від ситуації.

Для обов'язкових параметрів використовуються `ArgumentNullException.ThrowIfNull()` та `ArgumentNullException`.

Для перевірки формату електронної пошти використовується `EmailAddressAttribute`.

Для конкретних обмежень застосовуються звичайні перевірки `if`.

Такий підхід дозволяє виконувати перевірку даних безпосередньо на межі відповідного компонента та не передавати некоректні значення далі.

## 4. Загальні висновки

Під час виконання роботи було досліджено принципи інкапсуляції та валідації в реальному open-source проєкті ASP.NET Core.

На прикладі `DataProtectorTokenProvider<TUser>` було розглянуто властивості з `private set` та властивості тільки для читання. Такі механізми дозволяють контролювати доступ до внутрішнього стану об'єкта.

На прикладі `IdentityApiEndpointRouteBuilderExtensions` було розглянуто приватні поля, константи та внутрішній клас. Вони приховують деталі реалізації від зовнішнього коду.

Також було досліджено декілька підходів до валідації: перевірку `null`, `ArgumentNullException`, `EmailAddressAttribute` та перевірки за допомогою `if`.

Аналіз показав, що інкапсуляція та валідація доповнюють одна одну. Інкапсуляція обмежує доступ до внутрішніх даних, а валідація контролює коректність значень, які надходять до компонентів системи.

## 5. Контрольні запитання

### 1. За якими ознаками можна зробити висновок, що в класі дотримано інкапсуляції?

Основними ознаками є використання `private` полів, обмеження доступу до властивостей, `private set`, `readonly`, приватних внутрішніх класів та надання зовнішньому коду лише необхідних методів і властивостей.

### 2. Які підходи до валідації властивостей ви зустріли в реальному коді?

Було виявлено перевірки на `null`, використання `ArgumentNullException.ThrowIfNull()`, `ArgumentNullException`, `EmailAddressAttribute`, а також перевірки довжини рядка за допомогою `if`.

### 3. Чому для аналізу важливо наводити посилання на конкретні класи та фрагменти коду?

Конкретні посилання дозволяють перевірити наведені приклади та підтвердити, що аналіз виконано на реальному open-source коді. Фрагменти коду також показують практичне використання теоретичних принципів.

### 4. Які висновки щодо якості проєктування ви зробили після порівняння 2-3 класів?

Порівняння показало, що різні класи використовують різні механізми інкапсуляції та валідації відповідно до свого призначення. Спільним принципом є приховування внутрішньої реалізації та перевірка вхідних даних перед їх використанням.

## 6. Джерела

* **ASP.NET Core — GitHub:**
  https://github.com/dotnet/aspnetcore

* **DataProtectorTokenProvider.cs:**
  https://github.com/dotnet/aspnetcore/blob/main/src/Identity/Core/src/DataProtectorTokenProvider.cs

* **IdentityApiEndpointRouteBuilderExtensions.cs:**
  https://github.com/dotnet/aspnetcore/blob/main/src/Identity/Core/src/IdentityApiEndpointRouteBuilderExtensions.cs

* **Microsoft Learn — Access modifiers:**
  https://learn.microsoft.com/dotnet/csharp/language-reference/keywords/access-modifiers

* **Microsoft Learn — Properties:**
  https://learn.microsoft.com/dotnet/csharp/programming-guide/classes-and-structs/properties

## 7. Форма звітності

Результатом виконання самостійної роботи є файл `README.md` у форматі Markdown.

Файл містить назву обраного open-source проєкту, посилання на GitHub, посилання на конкретні файли, аналіз інкапсуляції, приклади валідації, фрагменти коду, відповіді на контрольні питання та загальні висновки.
