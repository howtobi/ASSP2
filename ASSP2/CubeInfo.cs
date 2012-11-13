using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AnalysisServices;
using Microsoft.AnalysisServices.AdomdServer;

namespace ASSP2
{
    public class CubeInfo
    {
        [SafeToPrepare(true)]
        public static DateTime GetCubeLastSchemaUpdateDate()
        {
            DateTime dtTemp = DateTime.MinValue;
            Exception exDelegate = null;

            string sServerName = Context.CurrentServerID;
            string sDatabaseName = Context.CurrentDatabaseName;
            string sCubeName = GetCurrentCubeName();

            System.Threading.Thread td = new System.Threading.Thread(delegate()
            {
                try {
                    Microsoft.AnalysisServices.Server oServer = new Microsoft.AnalysisServices.Server();
                    oServer.Connect("Data Source=" + sServerName);
                    Database db = oServer.Databases.GetByName(sDatabaseName);
                    Cube cube = db.Cubes.FindByName(sCubeName);

                    dtTemp = cube.LastSchemaUpdate;                
                }
                catch (Exception ex)
                {
                    exDelegate = ex;
                }
            }
            );
            td.Start();
            while (!td.Join(1000))
            {
                Context.CheckCancelled();
            }

            if (exDelegate != null) throw exDelegate;

            return dtTemp;
        }

        public static DateTime GetPartitionLastProcessedDate(string measureGroupName, string partitionName)
        {
            DateTime dtTemp = DateTime.MinValue;
            Exception exDelegate = null;

            if (string.IsNullOrEmpty(measureGroupName) || string.IsNullOrEmpty(partitionName))
                return dtTemp;

            string sServerName = Context.CurrentServerID;
            string sDatabaseName = Context.CurrentDatabaseName;
            string sCubeName = GetCurrentCubeName();
            string sMeasureGroupName = measureGroupName;
            string sPartitionName = partitionName;

            System.Threading.Thread td = new System.Threading.Thread(delegate()
            {
                try
                {
                    Microsoft.AnalysisServices.Server oServer = new Microsoft.AnalysisServices.Server();
                    oServer.Connect("Data Source=" + sServerName);
                    Database db = oServer.Databases.GetByName(sDatabaseName);
                    Cube cube = db.Cubes.FindByName(sCubeName);
                    MeasureGroup measuregroup = cube.MeasureGroups.FindByName(sMeasureGroupName);
                    Partition partition = measuregroup.Partitions.FindByName(sPartitionName);

                    dtTemp = partition.LastProcessed;
                }
                catch (Exception ex)
                {
                    exDelegate = ex;
                }
            }
            );
            td.Start();
            while (!td.Join(1000))
            {
                Context.CheckCancelled();
            }

            if (exDelegate != null) throw exDelegate;

            return dtTemp;
        }

        private static string GetCurrentCubeName()
        {
            string sCubeName = "";
            if (Context.CurrentCube != null)
            {
                sCubeName = Context.CurrentCube.Name;
            }
            else
            {
                sCubeName = new Expression("[Measures].CurrentMember.Properties(\"CUBE_NAME\")").Calculate(null).ToString();
            }

            //this code will run if the current cube is a perspective. it will return the name of the base cube in order to work around a bug in walking through the objects in AdomdServer in a perspective
            Property propBaseCubeName = Context.Cubes[sCubeName].Properties.Find("BASE_CUBE_NAME");
            if (propBaseCubeName != null)
                return Convert.ToString(propBaseCubeName.Value);
            else
                return sCubeName;
        }
    }
}
