using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.Student
{
    public record CreateNoteDto(
       string NoteText,
       string? NoteType
   );
}
