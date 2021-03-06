﻿namespace Application.Data.Entity
{
    public class DicomSliceEntity
    {
        public byte[] Image { get; set; }
        public byte[] Mask { get; set; }
        public int InstanceNumber { get; set; }
        public double SliceLocation { get; set; }
        public int DicomModelId { get; set; }

        public DicomModelEntity DicomModelEntity { get; set; }
    }
}