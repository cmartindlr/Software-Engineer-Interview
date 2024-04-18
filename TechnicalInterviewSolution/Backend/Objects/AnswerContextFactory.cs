using Backend.Models.Sql;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Objects
{
    /// <inheritdoc/>
    public class AnswerContextFactory : IDbContextFactory<AnswerContext>
    {
        /// <inheritdoc/>
        public AnswerContext CreateDbContext()
        {
            AnswerContext context = new AnswerContext();
            return context;
        }
    }
}
