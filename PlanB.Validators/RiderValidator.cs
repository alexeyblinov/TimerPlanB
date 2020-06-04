using FluentValidation;
using PlanB.BL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanB.Validators
{
    public class RiderValidator : AbstractValidator<Rider>
    {
        public RiderValidator()
        {
            ValidatorOptions.CascadeMode = FluentValidation.CascadeMode.StopOnFirstFailure;

            RuleFor(r => r.RiderId).InclusiveBetween(1, 99)
                                   .WithName("Номер участника")
                                   .WithMessage("{PropertyName} должен быть от 1 до 99.");
            RuleFor(r => r.Name).NotNull()
                                .MinimumLength(1)
                                .Must(name => name.All(ch => char.IsLetter(ch) || ch == '-'))
                                .WithName("Имя")
                                .WithMessage("{PropertyName} должно состоять из букв, составное имя может содержать дефис.");
            RuleFor(r => r.Surname).NotNull()
                                   .MinimumLength(1)
                                   .Must(surname => surname.All(ch => char.IsLetter(ch) || ch == '-'))
                                   .WithName("Фамилия")
                                   .WithMessage("{PropertyName} должна состоять из букв, составная фамилия может содержать дефис.");
            // Gender
            RuleFor(r => r.Location).NotNull()
                                    .MinimumLength(1)
                                    .Must(loc => loc.All(ch => char.IsLetter(ch) || ch == '-'))
                                    .WithName("Населённый пункт")
                                    .WithMessage("{PropertyName} должен состоять из букв, может включать дефис.");
            RuleFor(r => r.Team).NotNull()
                                .MinimumLength(1)
                                .Must(team => team.All(ch => char.IsLetter(ch) || ch == '-'))
                                .WithName("Команда")
                                .WithMessage("Название {PropertyName} должно состоять из букв, может включать дефис.");
            RuleFor(r => r.TryFirst).InclusiveBetween(0, Rider.MAXTIME)
                                    .WithName("Результат первой попытки")
                                    .WithMessage("{PropertyName} должен быть от 00:00:00 до 59:59:99.");
            RuleFor(r => r.TrySecond).InclusiveBetween(0, Rider.MAXTIME)
                                    .WithName("Результат второй попытки")
                                    .WithMessage("{PropertyName} должен быть от 00:00:00 до 59:59:99.");
            RuleFor(r => r.BestResult).InclusiveBetween(0, Rider.MAXTIME)
                                      .WithName("Лучшее время")
                                      .WithMessage("{PropertyName} должно быть от 00:00:00 до 59:59:99.");
            RuleFor(r => r.Rank).GreaterThan(0)
                                .WithName("Позиция")
                                .WithMessage("{PropertyName} должна быть натуральным числом.");
            RuleFor(r => r.PreviousClassId).NotNull()
                                           .Must(cl => Enum.IsDefined(typeof(ClassName), cl))
                                           .WithName("Класс участника")
                                           .WithMessage("{PropertyName} не определён.");
            RuleFor(r => r.ResultClassId).NotNull()
                                         .Must(cl => Enum.IsDefined(typeof(ClassName), cl))
                                         .WithName("Итоговый класс участника")
                                         .WithMessage("{PropertyName} не определён.");
        }
    }
}
