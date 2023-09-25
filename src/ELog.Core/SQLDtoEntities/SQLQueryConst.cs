namespace ELog.Core.SQLDtoEntities
{
    public class SQLQueryConst
    {
        public static readonly string PICKING_REPORT_SQL = @"
                                                                Select pickingHeader.Id, plantMaster.PlantId as PlantCode,pickingHeader.cubiclecode, cubicleAssignHeader.ProductCode,cubicleAssignHeader.GroupId,
                                                                processOrderMtrl.BatchNo,pickingHeader.MaterialCode,pickingDetail.SAPBatchNumber as SAPBatchNo,pickingDetail.Quantity,
                                                                pmmsLocation.LocationCode,pickingHeader.PickingTime,CONCAT(pmmsUser.FirstName,' ',pmmsUser.LastName) as PickedBy,pickingDetail.ContainerBarCode,
                                                                cubicleAssignDetail.InspectionLotId,cubicleAssignDetail.ProcessOrderId,plantMaster.Id as SubPlantId,
                                                                pickingHeader.IsSampling,pickingDetail.Id as NoOfContainers,po.ProcessOrderNo,lot.InspectionLotNumber as InspectionLotNo
                                                                from MaterialBatchDispensingHeaders pickingHeader
                                                                JOIN MaterialBatchDispensingContainerDetails pickingDetail on pickingHeader.Id=pickingDetail.MaterialBatchDispensingHeaderId
                                                                JOIN ProcessOrderMaterials processOrderMtrl on lOWER(processOrderMtrl.ItemCode)=lOWER(pickingHeader.MaterialCode)
                                                                JOIN CubicleAssignmentDetails cubicleAssignDetail on cubicleAssignDetail.ProcessOrderMaterialId=processOrderMtrl.Id
                                                                JOIN CubicleAssignmentHeader cubicleAssignHeader on pickingHeader.GroupCode=cubicleAssignHeader.GroupId                                                                left JOIN InspectionLot lot on lot.Id=processOrderMtrl.InspectionLotId
                                                                JOIN ProcessOrders po on po.Id=processOrderMtrl.ProcessOrderId
                                                                JOIN PlantMaster plantMaster on (po.PlantId=plantMaster.Id or lot.PlantId=plantMaster.Id)
                                                                JOIN GRNMaterialLabelPrintingContainerBarcodes grnMaterialContainer on pickingDetail.ContainerBarCode=grnMaterialContainer.MaterialLabelContainerBarCode
                                                                left JOIN Palletizations pallet on pallet.ContainerId=grnMaterialContainer.Id
                                                                JOIN PutAwayBinToBinTransfer putAway on (pallet.palletid=putAway.palletId or putAway.ContainerId=grnMaterialContainer.Id)
                                                                JOIN LocationMaster pmmsLocation on putAway.LocationId=pmmsLocation.Id
                                                                JOIN cubiclemaster pmmscubicle on pickingHeader.CubicleCode=pmmscubicle.CubicleCode
                                                                JOIN Users pmmsUser on pickingHeader.CreatedBy=pmmsUser.Id
                                                                where pickingHeader.MaterialBatchDispensingHeaderType=1 and pallet.IsUnloaded=0 and putAway.IsUnloaded=0";


        public static readonly string AUDIT_REPORT_SQL = @" 
SELECT  T.Id,T.TenancyName,PM.PlantName,UL.UserName AS CreatedBy,PM.CreatedOn,ULM.UserName AS ModifiedBy,PM.ModifiedOn,PM.[Description], PM.[Address1], 
		PM.[Address2], PM.[City], CM.CountryName, PM.[Email], PM.[GS1Prefix], PM.[License], PM.[PhoneNumber], PM.[PlantId], PM.[PostalCode], SM.StateName, PM.[TaxRegistrationNo], PM.[Website], 
		PM.[IsActive], PM.[PlantTypeId], PM.[MasterPlantId], ASM.ApprovalStatus, PM.[ApprovalStatusDescription],
		PM.SysStartTime,PM.SysEndTime
		from  [History].[PlantMaster] PM WITH (NOLOCK)
		INNER JOIN Tenants T WITH (NOLOCK) ON T.ID=PM.TenantId 
		INNER JOIN Users UL WITH (NOLOCK) ON UL.id=PM.CreatedBy
		LEFT JOIN StateMaster SM  WITH (NOLOCK) ON SM.ID=PM.StateId 
		LEFT JOIN  CountryMaster CM WITH (NOLOCK) ON CM.ID=PM.CountryId 
		LEFT JOIN  ApprovalStatusMaster ASM WITH (NOLOCK) ON ASM.ID=PM.ApprovalStatusId 
		LEFT JOIN Users ULM WITH (NOLOCK) ON ULM.id=PM.ModifiedBy";

        public static readonly string AUDIT_DYNAMIC_REPORT = @"exec Dynamic_SP 'Users', @param=''";

        public static readonly string AUDIT_MATERIAL_MASTER = @"exec Dynamic_SP 'MaterialMaster', @param=''";
    }



}
