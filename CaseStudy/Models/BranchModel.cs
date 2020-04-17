using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CaseStudy.Models
{
    public class BranchModel
    {
        private AppDbContext _db;
        public BranchModel(AppDbContext context)
        {
            _db = context;
        }

        internal object GetThreeClosestStores(float lat, float lng)
        {
            throw new NotImplementedException();
        }
        public List<Branch> GetThreeClosestBranches(float? lat, float? lng)
        {
            List<Branch> branchDetails = null;
            try
            {
                var latParam = new SqlParameter("@lat", lat);
                var lngParam = new SqlParameter("@lng", lng);
                var query = _db.Branches.FromSql("dbo.pGetThreeClosestBranches @lat, @lng", latParam, lngParam);
                branchDetails = query.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return branchDetails;
        }
        //public bool LoadCSVFromFile(string path)
        //{
        //    bool addWorked = false;
        //    try
        //    {
        //        // clear out the old rows
        //        _db.Stores.RemoveRange(_db.Stores);
        //        _db.SaveChanges();
        //        var csv = new List<string[]>();
        //        var csvFile = path + "\\exercisesStoreRaw.csv";
        //        var lines = System.IO.File.ReadAllLines(csvFile);
        //        foreach (string line in lines)
        //            csv.Add(line.Split(',')); // populate store with csv
        //        foreach (string[] x in csv)
        //        {
        //            Store aStore = new Store();
        //            aStore.Longitude = Convert.ToDouble(x[0]);
        //            aStore.Latitude = Convert.ToDouble(x[1]);
        //            aStore.Street = x[2];
        //            aStore.City = x[3];
        //            aStore.Region = x[4];
        //            _db.Stores.Add(aStore);
        //            _db.SaveChanges();
        //        }
        //        addWorked = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return addWorked;
        //}

    }
}
