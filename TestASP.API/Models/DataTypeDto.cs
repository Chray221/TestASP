using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TestASP.Data.Enums;

namespace TestASP.API.Models
{
	public class DataTypeDto: BaseDto
    {
        public Guid Guid { get; set; }
        public byte Byte { get; set; }
        public sbyte SByte { get; set; }
        public ushort USHort { get; set; }
        public short Short { get; set; }
        public uint UInt { get; set; }
        public int Int { get; set; }
        public ulong ULong { get; set; }
        public long Long { get; set; }
        public float Float { get; set; }
        public double Double { get; set; }
        [DataType(DataType.Currency)]
        public double Currency { get; set; }

        public decimal Decimal { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = string.Empty;
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [DataType(DataType.Text)]
        public string Text { get; set; } = string.Empty;

        // nvarchar
        public string MaxNvarchar { get; set; } = string.Empty;
        [Column(TypeName = "nvarchar(50)")]
        public string LimitNvarchar { get; set; } = string.Empty;

        public GenderEnum GenderEnum { get; set; }
        public GenderByteEnum GenderByteEnum { get; set; }
        public GenderByteEnum GenderStr { get; set; }
        public DataTypeDto()
		{
		}
	}
}

