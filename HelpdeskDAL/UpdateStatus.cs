/*
 * Module Title: UpdateStatus.cs
 * Coder: Kenneth Rose
 * Purpose: Status return for stale updates
 * Date: Oct. 27, 2024
 */

using System.Text;
using System.Threading.Tasks;

namespace HelpdeskDAL
{
    public enum UpdateStatus
    {
        Ok = 1,
        Failed = -1,
        Stale = -2
    }
}

