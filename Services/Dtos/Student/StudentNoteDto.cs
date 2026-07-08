using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.Student
{
    public record StudentNoteDto(
           int NoteID,
           int StudentID,
           int UserID,
           string? NoteText,
           string? NoteType,
           DateTime CreatedAt
       );
}
