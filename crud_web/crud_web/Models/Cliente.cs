using System.ComponentModel.DataAnnotations;

namespace crud_web.Models
{
    public class Cliente
    {
        
        [Key]
        public int id { get; set; }
        public string nome { get; set; }
        public string dt_nascimento { get; set; }
        public string telefone { get; set; }
        public string email { get; set; }
        public string cidade { get; set; }
        
    }
}

