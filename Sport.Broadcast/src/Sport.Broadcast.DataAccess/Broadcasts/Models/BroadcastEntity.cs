using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Enums;

namespace Sport.Broadcast.DataAccess.Broadcasts.Models
{
    [Table("football", Schema = "broadcast")]
    internal class BroadcastEntity
    {
        /// <summary>
        /// Айди трансляции.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }
        
        /// <summary>
        /// Название команды, которая играет дома.
        /// </summary>
        [Column("home_team")]
        [Required]
        public string HomeTeam { get; set; }
        
        /// <summary>
        /// Название команды, которая играет в гостях.
        /// </summary>
        [Column("guest_team")]
        [Required]
        public string GuestTeam { get; set; }
        
        /// <summary>
        /// Время начала трансляции.
        /// </summary>
        [Column("start_time")]
        [Required]
        public DateTime StartTime { get; set; }
        
        /// <summary>
        /// Количество забитых голов со стороны домашней команды.
        /// </summary>
        [Column("score_of_home_team")]
        public int? ScoreOfHomeTeam { get; set; }
        
        /// <summary>
        /// Количество забитых голов со стороны гостевой команды.
        /// </summary>
        [Column("score_of_guest_team")]
        public int? ScoreOfGuestTeam { get; set; }
        
        /// <summary>
        /// Текующий тайм.
        /// </summary>
        [Column("current_half")]
        public int? CurrentHalf { get; set; }

        /// <summary>
        /// Текующее дополнительное игровое время тайма в минутах.
        /// </summary>
        [Column("extra_time")]
        public int? ExtraTime { get; set; }
        
        /// <summary>
        /// Общее дополнительное игровое время матча в минутах.
        /// </summary>
        [Column("total_extra_time")]
        public int? TotalExtraTime { get; set; }
        
        /// <summary>
        /// Статус трансляции.
        /// </summary>
        [Column("status")]
        [Required]
        public BroadcastStatus Status { get; set; }
    }
}