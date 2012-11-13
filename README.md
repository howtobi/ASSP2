# ASSP2

This Project is forked from [Analysis Services Stored Procedure Project](http://asstoredprocedures.codeplex.com/)

I want to Partition Processed Date so I make it.

## Analysis Services Stored Procedure Project

    * http://asstoredprocedures.codeplex.com/

## Two Reference for MSSQL 2012

    * 1) Microsoft.AnalysisServices - C:\Program Files\Microsoft SQL Server\110\SDK\Assemblies\Microsoft.AnalysisServices.DLL
    * 2) msmgdsrv(AS Server) - C:\Program Files\Microsoft Analysis Services\AS OLEDB\110\msmgdsrv.dll

## Usage

    * registe assp2.dll
	* http://asstoredprocedures.codeplex.com/wikipage?title=Installation%20Instructions&referringTitle=Home
    * GetCubeLastSchemaUpdateDate - no parameter
    * GetPartitionLastProcessedDate - measure group, partition name

## Sample

'''WITH MEMBER [MEASURES].[LASTSCHEMAUPDATE]
AS ASSP2.GetCubeLastSchemaUpdateDate()
MEMBER [MEASURES].[LASTPROCESSED]
AS ASSP2.GetPartitionLastProcessedDate("Internet Sales", "Internet_Sales")
SELECT {[MEASURES].[LASTSCHEMAUPDATE], [MEASURES].[LASTPROCESSED]} ON 0
FROM [Adventure Works]
'''

## About

I do not understand ASSP license.

So if I make the mistake, know me.

Of course, I can make decision this project's license, It will be the MIT license.

Thank you.