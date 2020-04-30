using UnityEngine;
using System.Collections;
using System;
using System.IO;
using Oculus.Avatar;
using System.Runtime.InteropServices;
using System.Collections.Generic;

public class Playback : MonoBehaviour
{
    [Serializable]
    class PacketsFile
    {
        public LinkedList<Packet> packetList;
    };

    [Serializable]
    class Packet
    {
        public byte[] PacketData;
    };

    public OvrAvatar LocalAvatar;
    public OvrAvatar LoopbackAvatar;

    private int PacketSequence = 0;

    LinkedList<Packet> packetQueue = new LinkedList<Packet>();

    public bool record = false;
    bool _lastRecord = false;
    public bool playback = false;
    public string fileName = null;
    LinkedList<Packet> _recordedQueue = new LinkedList<Packet>();

    int sequence =0;
    void Start()
    {
        if (playback)
        {
            ReadFile();
            StartCoroutine(PlayFile());
            
        }
        else
        {
            LocalAvatar.RecordPackets = true;
            LocalAvatar.PacketRecorded += OnLocalAvatarPacketRecorded;
            _lastRecord = record;
        }
    }

    IEnumerator PlayFile() {


        LinkedListNode<Packet> packet = _recordedQueue.First;
        if (packet == null)
        {
            ReadFile();
            packet = _recordedQueue.First;
        }


        while (packet.Next != null)
        {
            SendPacketData(packet.Value.PacketData);
            packet = packet.Next;

            yield return new WaitForSeconds(0.05f);

        }

        StartCoroutine(PlayFile());

    }

    void OnLocalAvatarPacketRecorded(object sender, OvrAvatar.PacketEventArgs args)
    {
        if (playback)
        {
            LinkedListNode<Packet> packet = _recordedQueue.First;
            if (packet == null)
            {
                ReadFile();
                packet = _recordedQueue.First;
            }
            SendPacketData(packet.Value.PacketData);
            _recordedQueue.RemoveFirst();

        }

        else using (MemoryStream outputStream = new MemoryStream())
            {
                BinaryWriter writer = new BinaryWriter(outputStream);

                var size = CAPI.ovrAvatarPacket_GetSize(args.Packet.ovrNativePacket);
                byte[] data = new byte[size];
                CAPI.ovrAvatarPacket_Write(args.Packet.ovrNativePacket, size, data);

                writer.Write(PacketSequence++);
                writer.Write(size);
                writer.Write(data);

                SendPacketData(outputStream.ToArray());
            }
    }

    void Update()
    {

        if (!record && _lastRecord)
        {
            WriteToFile();
            _lastRecord = record;
        }

        if (packetQueue.Count > 0)
        {
            List<Packet> deadList = new List<Packet>();
            foreach (Packet packet in packetQueue)
            {
                ReceivePacketData(packet.PacketData);
                deadList.Add(packet);
            }

            foreach (var packet in deadList)
            {
                packetQueue.Remove(packet);
            }
        }
    }

    void SendPacketData(byte[] data)
    {
        Packet packet = new Packet();
        packet.PacketData = data;

        packetQueue.AddLast(packet);
        if (record && !playback) _recordedQueue.AddLast(packet);

    }

    void ReceivePacketData(byte[] data)
    {
        using (MemoryStream inputStream = new MemoryStream(data))
        {
            BinaryReader reader = new BinaryReader(inputStream);
            int sequence = reader.ReadInt32();
            

            int size = reader.ReadInt32();
            byte[] sdkData = reader.ReadBytes(size);

            IntPtr packet = CAPI.ovrAvatarPacket_Read((UInt32)data.Length, sdkData);
            LoopbackAvatar.GetComponent<OvrAvatarRemoteDriver>().QueuePacketLoop(sequence, new OvrAvatarPacket { ovrNativePacket = packet });
          
        }
    }

    void WriteToFile()
    {
        using (Stream stream = File.Open(fileName, FileMode.Create))
        {
            new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter().Serialize(stream, new PacketsFile { packetList = _recordedQueue });
        }
        Debug.Log("File written");
    }

    void ReadFile()
    {
        fileName = Path.Combine(Application.streamingAssetsPath, fileName);

        fileName.Replace("\\", "/");

        using (Stream stream = File.Open(fileName, FileMode.Open))
        {
            _recordedQueue = (new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter().Deserialize(stream) as PacketsFile).packetList;

        }
        Debug.Log("File read");
    }
}
