using AD.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repositories.Interfaces
{
    public interface IHomeRepository
    {
        Task LogToFile(LogDTO log);
        Task LogToDB(LogDTO log);
    }
}
