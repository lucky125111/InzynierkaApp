﻿using System;
using System.Collections.Generic;
using System.Linq;
using Core.Context;
using Core.Entity;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories
{
    public class DicomSliceRepository : IDicomSliceRepository
    {
        private readonly DicomContext _dicomContext;

        public DicomSliceRepository(DicomContext dicomContext)
        {
            _dicomContext = dicomContext;
        }

        public IEnumerable<byte[]> GetSlices(int patientId)
        {
            var dicomSlices = _dicomContext.DicomSlices.Where(x => x.DicomModelId == patientId);
            return !dicomSlices.Any() ? null : dicomSlices.Select(x => x.Image).AsEnumerable<byte[]>();
        }

        public IEnumerable<byte[]> GetMasks(int patientId)
        {
            var dicomSlices = _dicomContext.DicomSlices.Where(x => x.DicomModelId == patientId);
            return !dicomSlices.Any() ? null : dicomSlices.Select(x => x.Mask).AsEnumerable<byte[]>();
        }

        public DicomSlice GetDicomSlice(int patientId, int sliceId)
        {
            return _dicomContext.DicomSlices.Find(patientId, sliceId);
        }

        public byte[] GetSliceById(int patientId, int sliceId)
        {
            return  _dicomContext.DicomSlices.Find(patientId, sliceId)?.Image;
        }

        public byte[] GetMaskById(int patientId, int sliceId)
        {            
            return _dicomContext.DicomSlices.Find(patientId, sliceId)?.Mask;
        }

        public void UpdateMask(DicomSlice dicomSlice)
        {
            _dicomContext.Entry(dicomSlice).State = EntityState.Modified;
        }

        public void InsertSlice(DicomSlice dicomSlice)
        {
            _dicomContext.DicomSlices.Add(dicomSlice);
        }

        public void InsertSlices(IEnumerable<DicomSlice> dicomSlices)
        {
            foreach (var slice in dicomSlices)
            {
                InsertSlice(slice);
            }
        }

        public void Save()
        {
            _dicomContext.SaveChanges();
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _dicomContext.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}