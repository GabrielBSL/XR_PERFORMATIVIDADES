using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;

namespace Main.Debugs
{
    public class KitMotionPrinter : MonoBehaviour
    {
        public TextAsset xmlFile;
        public float gizmoRadius = .5f;
        public Vector3 rootPos;

        private XElement motionFrames;

        private string heightString;
        private float height;

        private Vector3 bln;
        private Vector3 bp;
        private Vector3 bt;
        private Vector3 bun;

        private Vector3 la;
        private Vector3 le;
        private Vector3 lh;
        private Vector3 lk;
        private Vector3 ls;
        private Vector3 lw;
        private Vector3 lf;
        private Vector3 lmroot;

        private Vector3 ra;
        private Vector3 re;
        private Vector3 rh;
        private Vector3 rk;
        private Vector3 rs;
        private Vector3 rw;
        private Vector3 rf;
        private Vector3 rmroot;

        private Vector3 curbln;
        private Vector3 curbp;
        private Vector3 curbt;
        private Vector3 curbun;

        private Vector3 curla;
        private Vector3 curle;
        private Vector3 curlh;
        private Vector3 curlk;
        private Vector3 curls;
        private Vector3 curlw;
        private Vector3 curlf;
        private Vector3 curlmroot;

        private Vector3 curra;
        private Vector3 curre;
        private Vector3 currh;
        private Vector3 currk;
        private Vector3 currs;
        private Vector3 currw;
        private Vector3 currf;
        private Vector3 currmroot;

        // Start is called before the first frame update
        void Start()
        {
            if(xmlFile == null)
            {
                return;
            }

            XElement root = XElement.Parse(xmlFile.text);

            foreach(XElement motion in root.Elements("Motion"))
            {
                string motionName = motion.Attribute("name")?.Value;
                Debug.Log("Motion Name: " + motionName);

                // Read the Model data
                var model = motion.Element("Model");
                if (model != null)
                {
                    string modelFile = model.Element("File")?.Value;
                    Debug.Log("Model File: " + modelFile);
                }

                // Read the MotionFrames data
                motionFrames = motion.Element("MotionFrames");

                heightString = motion.Element("ModelProcessorConfig").Element("Height").Value;
                heightString = heightString.Replace('.', ',');

                height = float.Parse(heightString);
            }
        }

        private void Update()
        {
            if(motionFrames == null)
            {
                return;
            }

            foreach (XElement frame in motionFrames.Elements("MotionFrame"))
            {
                string jointList = frame.Element("JointPosition").Value;
                string[] jointValues = jointList.Split(' ');
                float[] jointFloat = new float[jointValues.Length];

                for (int i = 0; i < jointValues.Length; i++)
                {
                    jointFloat[i] = float.Parse(jointValues[i]);
                }

                bln = new Vector3(jointFloat[0], jointFloat[1], jointFloat[2]);
                bp = new Vector3(jointFloat[3], jointFloat[4], jointFloat[5]);
                bt = new Vector3(jointFloat[6], jointFloat[7], jointFloat[8]);
                bun = new Vector3(jointFloat[9], jointFloat[10], jointFloat[10]);

                la = new Vector3(jointFloat[12], jointFloat[13], jointFloat[14]);
                le = new Vector3(jointFloat[15], jointFloat[16]);
                lh = new Vector3(jointFloat[17], jointFloat[18], jointFloat[19]);
                lk = new Vector3(jointFloat[20], 0);
                ls = new Vector3(jointFloat[21], jointFloat[22], jointFloat[23]);
                lw = new Vector3(jointFloat[24], jointFloat[25]);
                lf = new Vector3(jointFloat[26], 0);
                lmroot = new Vector3(jointFloat[27], 0);

                ra = new Vector3(jointFloat[28], jointFloat[29], jointFloat[30]);
                re = new Vector3(jointFloat[31], jointFloat[32]);
                rh = new Vector3(jointFloat[33], jointFloat[34], jointFloat[35]);
                rk = new Vector3(jointFloat[36], 0);
                rs = new Vector3(jointFloat[37], jointFloat[38], jointFloat[39]);
                rw = new Vector3(jointFloat[40], jointFloat[41]);
                rf = new Vector3(jointFloat[42], 0);
                rmroot = new Vector3(jointFloat[43], 0);
            }
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
            {
                if (xmlFile == null)
                {
                    return;
                }

                XElement root = XElement.Parse(xmlFile.text);

                foreach (XElement motion in root.Elements("Motion"))
                {
                    // Read the MotionFrames data
                    motionFrames = motion.Element("MotionFrames");

                    heightString = motion.Element("ModelProcessorConfig").Element("Height").Value;
                    heightString = heightString.Replace('.', ',');
               
                    height = float.Parse(heightString);
                }

                if (motionFrames == null)
                {
                    return;
                }

                foreach (XElement frame in motionFrames.Elements("MotionFrame"))
                {
                    string jointList = frame.Element("JointPosition").Value;
                    jointList = jointList.Replace('.', ',');
                    string[] jointValues = jointList.Split(' ');
                    float[] jointFloat = new float[jointValues.Length - 1];

                    for (int i = 0; i < jointValues.Length - 1; i++)
                    {
                        jointFloat[i] = float.Parse(jointValues[i]);
                    }

                    bln = new Vector3(jointFloat[0], jointFloat[1], jointFloat[2]) + rootPos + new Vector3(0, .31f * height);
                    bp = new Vector3(jointFloat[3], jointFloat[4], jointFloat[5]) + rootPos + new Vector3(0, .04f * height);
                    bt = new Vector3(jointFloat[6], jointFloat[7], jointFloat[8]) + rootPos + new Vector3(0, .1f * height);
                    bun = new Vector3(jointFloat[9], jointFloat[10], jointFloat[11]) + rootPos + new Vector3(0, .34f * height);

                    la = new Vector3(jointFloat[12], jointFloat[13], jointFloat[14]);
                    le = new Vector3(jointFloat[15], jointFloat[16]);
                    lh = new Vector3(jointFloat[17], jointFloat[18], jointFloat[19]);
                    lk = new Vector3(jointFloat[20], 0);
                    ls = new Vector3(jointFloat[21], jointFloat[22], jointFloat[23]);
                    lw = new Vector3(jointFloat[24], jointFloat[25]);
                    lf = new Vector3(jointFloat[26], 0);
                    lmroot = new Vector3(jointFloat[27], 0);

                    ra = new Vector3(jointFloat[28], jointFloat[29], jointFloat[30]);
                    re = new Vector3(jointFloat[31], jointFloat[32]);
                    rh = new Vector3(jointFloat[33], jointFloat[34], jointFloat[35]);
                    rk = new Vector3(jointFloat[36], 0);
                    rs = new Vector3(jointFloat[37], jointFloat[38], jointFloat[39]);
                    rw = new Vector3(jointFloat[40], jointFloat[41]);
                    rf = new Vector3(jointFloat[42], 0);
                    rmroot = new Vector3(jointFloat[43], 0);

                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(rootPos, gizmoRadius);

                    Vector3 curRootPos = rootPos;
                    Gizmos.color = Color.red;

                    Gizmos.DrawSphere(bp, gizmoRadius);
                    Gizmos.DrawLine(bp, curRootPos);

                    Gizmos.DrawSphere(bt, gizmoRadius);
                    Gizmos.DrawLine(bp, bt);

                    Gizmos.DrawSphere(bln, gizmoRadius);
                    Gizmos.DrawLine(bt, bln);

                    Gizmos.DrawSphere(bun, gizmoRadius);
                    Gizmos.DrawLine(bun, bln);

                    /*
                    Vector3 rootPosBackup = curRootPos;

                    curRootPos += rs;
                    Gizmos.DrawWireSphere(curRootPos, gizmoRadius);
                    Gizmos.DrawLine(curRootPos - rs, curRootPos);

                    curRootPos += re;
                    Gizmos.DrawWireSphere(curRootPos, gizmoRadius);
                    Gizmos.DrawLine(curRootPos - re, curRootPos);

                    curRootPos += rw;
                    Gizmos.DrawWireSphere(curRootPos, gizmoRadius);
                    Gizmos.DrawLine(curRootPos - rw, curRootPos);

                    curRootPos = rootPosBackup;

                    curRootPos += ls;
                    Gizmos.DrawWireSphere(curRootPos, gizmoRadius);
                    Gizmos.DrawLine(curRootPos - ls, curRootPos);

                    curRootPos += le;
                    Gizmos.DrawWireSphere(curRootPos, gizmoRadius);
                    Gizmos.DrawLine(curRootPos - le, curRootPos);

                    curRootPos += lw;
                    Gizmos.DrawWireSphere(curRootPos, gizmoRadius);
                    Gizmos.DrawLine(curRootPos - lw, curRootPos);

                    curRootPos = rootPosBackup;

                    curRootPos += bln;
                    Gizmos.DrawWireSphere(curRootPos, gizmoRadius);
                    Gizmos.DrawLine(curRootPos - bln, curRootPos);

                    curRootPos += bun;
                    Gizmos.DrawWireSphere(curRootPos, gizmoRadius);
                    Gizmos.DrawLine(curRootPos - bun, curRootPos);
*/
                    break;
                }
            }
        }
    }
}
