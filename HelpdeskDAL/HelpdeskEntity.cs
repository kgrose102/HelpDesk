/*
 * Module Title: HelpdeskEntity.cs
 * Coder: Kenneth Rose
 * Purpose: Creates and initializes the helpdeskentity with the ID and Timer
 * Date: Oct. 27, 2024
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace HelpdeskDAL
{
    public class HelpdeskEntity
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[]? Timer { get; set; }
    }
}

