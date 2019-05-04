using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookOrganizer.Domain
{
    public class SeriesReadOrder
    {
        private int _instalment;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid BookId { get; set; }
        public Guid SeriesId { get; set; }
        public int Instalment
        {
            get => _instalment;
            set
            {
                if (SeriesId != Guid.Empty)
                {
                    if (value > 0)
                    {
                        _instalment = value;
                    }
                    else
                        throw new ArgumentOutOfRangeException(nameof(Instalment),
                            "Instalment number must be greater than zero!");
                }
                else
                {
                    throw new ArgumentException("SeriesId not set!", nameof(SeriesId));
                }
            }
        }

        // Navigation properties
        public Book Book { get; set; }
        public Series Series { get; set; }
    }
}
