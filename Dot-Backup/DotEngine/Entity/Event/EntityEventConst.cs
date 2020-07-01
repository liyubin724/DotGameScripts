using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Entity.Event
{
    public static class EntityEventConst
    {
        public static int ENTITY_CREATE_EVENT = 1000;
        public static int ENTITY_DELETE_EVENT = 10001;
        public static int ENTITY_POSITION_CHANGE_EVENT = 1002;
        public static int ENTITY_FORWARD_CHANGED_EVENT = 1003;
        public static int ENTITY_ROTATION_CHANGED_EVENT = 1004;
        public static int ENTITY_AVATAR_SKELETON_CHANGED_EVENT = 1005;
        public static int ENTITY_AVATAR_ASSEMBLE_PART_EVENT = 1006;
        public static int ENTITY_AVATAR_DISASSEMBLE_PART_EVENT = 1007;



        public static int ENTITY_POSITION_CHANGED = 100;
        public static int ENTITY_ROTATION_CHANGED = 101;
        public static int ENTITY_FORWARD_CHANGED = 102;

        public static int ENTITY_SKELETON_LOAED = 200;
        public static int ENTITY_PART_LOAED = 201;
        public static int ENTITY_SKELETON_UNLOAD = 202;
        public static int ENTITY_PART_UNLOAD = 203;


    }
}
