using System.ComponentModel;

namespace Sport.Broadcast.AppServices.Contract.Broadcasts.SignalR.Enums
{
    public enum EventInGame
    {
        /// <summary>
        /// Забили гол.
        /// </summary>
        [Description("Гол")]
        Goal,

        /// <summary>
        /// Проверка VAR.
        /// </summary>
        [Description("VAR")]
        Var,
        
        /// <summary>
        /// VAR отменил гол.
        /// </summary>
        [Description("VAR")]
        VarGoalCancellation,

        /// <summary>
        /// Замена.
        /// </summary>
        [Description("Замена")]
        Substitution,
        
        /// <summary>
        /// Начало следующего тайма.
        /// </summary>
        [Description("Начало тайма")]
        BeginningHalf,

        /// <summary>
        /// Желтая карточка.
        /// </summary>
        [Description("Желтая карточка")]
        YellowCard,

        /// <summary>
        /// Вторая желтая карточка.
        /// </summary>
        [Description("Вторая желтая карточка")]
        SecondYellowCard,

        /// <summary>
        /// Красная карточка.
        /// </summary>
        [Description("Красная карточка")]
        RedCard,
        
        /// <summary>
        /// Дали дополнительное время.
        /// </summary>
        [Description("Дополнительное время")]
        ExtraTime,
        
        /// <summary>
        /// Перерыв.
        /// </summary>
        [Description("Перерыв")]
        Break
    }
}