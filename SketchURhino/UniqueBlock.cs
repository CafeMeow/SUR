namespace SketchURhino
{
    public class UniqueBlock : Command
    {
        public UniqueBlock()
        {
            // Rhino only creates one instance of each command class defined in a
            // plug-in, so it is safe to store a refence in a static property.
            Instance = this;

        }

        ///<summary>The only instance of this command.</summary>
        public static UniqueBlock Instance { get; private set; }

        ///<returns>The command name as it appears on the Rhino command line.</returns>
        public override string EnglishName => "SURUniqueBlock";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // TODO: start here modifying the behaviour of your command.
            // ---
            //RhinoApp.WriteLine("The {0} command will Unique Block .", EnglishName);
            GetObject gs = new GetObject();
            gs.GeometryFilter = ObjectType.InstanceReference;
            gs.GetMultiple(0, 0);
            blocksGeo.Clear();
            transforms.Clear();
            atts.Clear();
            blocksatts.Clear();
            if (gs.Result() == GetResult.Object)
            {
                Guid parentId = (gs.Object(0).Object().Geometry as InstanceReferenceGeometry).ParentIdefId;
                foreach (ObjRef objRef in gs.Objects())
                {
                    if ((objRef.Object().Geometry as InstanceReferenceGeometry).ParentIdefId != parentId)
                    {
                        RhinoApp.WriteLine("不是选的同一类的块");
                        return Result.Failure;

                    }
                    transforms.Add((objRef.Object().Geometry as InstanceReferenceGeometry).Xform);
                    blocksatts.Add(objRef.Object().Attributes);
                }
                foreach (ObjRef objRef2 in gs.Objects())
                {
                    doc.Objects.Delete(objRef2, false, true);
                }
                InstanceDefinition instanceDefinition = doc.InstanceDefinitions.FindId(parentId);
                foreach (RhinoObject rhinoObject in instanceDefinition.GetObjects())
                {
                    blocksGeo.Add(rhinoObject.Geometry);
                    atts.Add(rhinoObject.Attributes);
                }
                // See if block name already exists
                for (int i = 1; i < 1000; i++)
                {
                    newBlockName = instanceDefinition.Name + "#" + i;
                    if (doc.InstanceDefinitions.Find(newBlockName) != null)
                        continue;
                    break;
                }
                int num = doc.InstanceDefinitions.Add(newBlockName, instanceDefinition.Description, new Point3d(0.0, 0.0, 0.0), blocksGeo, atts);
                for (int l = 0; l < transforms.Count; l++)
                {
                    doc.Objects.AddInstanceObject(num, transforms[l], blocksatts[l]);
                }

                RhinoApp.WriteLine("图块 {0} 独立为 {1} .", instanceDefinition.Name, newBlockName);
            }
            return Result.Success;

        }
        private string newBlockName;
        private List<GeometryBase> blocksGeo = new List<GeometryBase>();
        private List<Transform> transforms = new List<Transform>();
        private List<ObjectAttributes> blocksatts = new List<ObjectAttributes>();
        private List<ObjectAttributes> atts = new List<ObjectAttributes>();
    }
}
