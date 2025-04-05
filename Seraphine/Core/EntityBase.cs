using System.ComponentModel.DataAnnotations;

namespace Seraphine.Core;

public class EntityBase
{
    [Key] public Guid Id { get; set; }

    public Guid? UsuarioCriacaoId { get; set; }

    public DateTime? DataHoraCriacao { get; set; }
    public DateTime? DataHoraAlteracao { get; set; }
}