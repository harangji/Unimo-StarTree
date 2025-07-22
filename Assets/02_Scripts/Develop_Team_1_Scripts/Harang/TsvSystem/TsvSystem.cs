// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Globalization;
// using System.IO;
// using System.Linq;
// using System.Text;
// using UnityEngine;
// using CsvHelper;
// using CsvHelper.Configuration;
//
// // public interface IKeyedData
// // {
// //     string Key { get; }
// // }
//
// public class TsvSystem : SingletonBehaviour<TsvSystem>
// {
//     // public static TsvSystem Instance { get; private set; }
//     //
//     // private void Awake()
//     // {
//     //     Instance = this;
//     // }
//     
//     public void WriteRecordsToTsv<T>(IEnumerable<T> records, string fileName)
//     {
//         if (records == null || !records.Any())
//         {
//             // Debug.LogWarning("No records to write.");
//             return;
//         }
//
//         string path = Path.Combine(Application.streamingAssetsPath, fileName);
//
//         var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
//         {
//             Delimiter = "\t",
//             HasHeaderRecord = true,
//             Mode = CsvMode.NoEscape,
//             BadDataFound = null
//         };
//         
//         using (var writer = new StreamWriter(path, false, Encoding.UTF8))
//         using (var csv = new CsvWriter(writer, csvConfig))
//         {
//             csv.Context.TypeConverterCache.AddConverter<int[]>(new IntArrayConverter());  //int�迭 �Ľ�
//             csv.WriteRecords(records);
//         }
//         
//         // Debug.Log($"Successfully wrote {records.Count()} records to TSV at {path}");
//     }
//     
//     /*public void WritePlayersToTsv(FieldPlayer.PlayerInfo[] players)
//     {
//         if (players == null || players.Length == 0)
//         {
//             Debug.LogWarning("No players to write.");
//             return;
//         }
//
//         string path = Path.Combine(Application.streamingAssetsPath, "PlayerTable.tsv");
//
//         using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
//         {
//             // Header
//             writer.WriteLine("Key\tName\tCurrentHp\tMaxHp\thasSkills\tequippedSkills\tPositionX\tPositionY\tPositionZ\tAngleX\tAngleY\tAngleZ");
//
//             // Body
//             foreach (var player in players)
//             {
//                 writer.WriteLine(player.ToTsv());
//             }
//         }
//
//         Debug.Log($"Successfully wrote {players.Length} players to TSV at {path}");
//     }*/
//
//     public T[] ReadRecordsFromTsv<T>(string fileName, string? targetKey = null) where T : IKeyedData //���׸� Ÿ�Կ� Key�Ӽ��� ����
//     {
//         T[] recordsArray = null;
//
//         var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
//         {
//             Delimiter = "\t",
//             HasHeaderRecord = true,
//             Mode = CsvMode.NoEscape,
//             BadDataFound = null
//         };
//
//         string path = Path.Combine(Application.streamingAssetsPath, fileName);
//
//         if (!File.Exists(path))
//         {
//             // Debug.LogWarning($"File not found: {path}");
//             return null;
//         }
//
//         using (var reader = new StreamReader(path))
//         using (var csv = new CsvReader(reader, csvConfig))
//         {
//             csv.Context.TypeConverterCache.AddConverter<int[]>(new IntArrayConverter());
//
//             var records = csv.GetRecords<T>();
//
//             // Ű�� �ִٸ� �ش� Ű�� �ش��ϴ� ���ڵ常 ���͸�
//             if (targetKey != null)
//             {
//                 var matched = records.FirstOrDefault(r => r.Key == targetKey);
//                 recordsArray = matched != null ? new T[] { matched } : Array.Empty<T>();
//             }
//             else
//             {
//                 recordsArray = records.ToArray();
//             }
//         }
//
//         MyDebug.Log(recordsArray.Length > 0
//             ? $"Success Read {typeof(T).Name}s ::: {path}"
//             : $"No records found in {path} (Key: {targetKey})");
//
//         return recordsArray;
//     }
//
//     //-------------------------------------------------------------
//     public T[] ReadRecordsFromTsv<T>(string fileName)
//     {
//         T[] recordsArray = null;
//
//         var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
//         {
//             Delimiter = "\t",           //�����ڸ� tab����
//             HasHeaderRecord = true,     //ù ���� Header�̴�.
//             Mode = CsvMode.NoEscape,    //�̽������� ���ڰ� ���� ( ", @, $ ...�� �־ ȿ���� �����ϰ� �ؽ�Ʈ�� �ν� )
//             BadDataFound = null         //�߸��� �����͸� ����
//         };
//
//         string path = Path.Combine(Application.streamingAssetsPath, fileName);
//
//         if (!File.Exists(path))
//         {
//             // Debug.LogWarning($"File not found: {path}");
//             return null;
//         }
//
//         //using�� Dispose()�� ��������� ȣ���Ѵ�. Dispose()�� ����������Close()�� �����ϸ�
//         //�̴� ������ �� ���� �� �ڵ����� ���� ��Ʈ���� �ݾ��ִ� ������ ����̴�.
//         //���� ������ �÷��͸� ��ٸ��� �ʰ� �ٷ� �����ϱ� ������ �� ������.
//         using (var reader = new StreamReader(path))
//         using (var csv = new CsvReader(reader, csvConfig))
//         {
//             csv.Context.TypeConverterCache.AddConverter<int[]>(new IntArrayConverter()); //int�迭 �Ľ�
//             
//             var records = csv.GetRecords<T>(); //CsvHelper�� �ڵ����� ������ Ŭ������ �Ӽ����� ������ ��ü ����Ʈ�� ����
//             recordsArray = records.ToArray();
//         }
//
//         MyDebug.Log(recordsArray.Length > 0
//             ? $"Success Read {typeof(T).Name}s ::: {path}"
//             : $"No records found in {path}");
//
//         return recordsArray;
//     }
//
//
//     /*private Player[] GetPlayersOrNull()
//     {
//         Player[] players = null;
//         
//         var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture) //���ڳ� ��¥ ���� ������ ������ �۷ι��� ����
//         {
//             Delimiter = "\t",           //�����ڸ� tab����
//             HasHeaderRecord = true,     //ù ���� Header�̴�.
//             Mode = CsvMode.NoEscape,    //�̽������� ���ڰ� ���� ( ", @, $ ...�� �־ ȿ���� �����ϰ� �ؽ�Ʈ�� �ν� )
//             BadDataFound = null         //�߸��� �����͸� ����
//         };
//
//         string path = Path.Combine(Application.streamingAssetsPath, "PlayerTable.tsv");
//
//         //using�� Dispose()�� ��������� ȣ���Ѵ�. Dispose()�� ����������Close()�� �����ϸ�
//         //�̴� ������ �� ���� �� �ڵ����� ���� ��Ʈ���� �ݾ��ִ� ������ ����̴�.
//         //���� ������ �÷��͸� ��ٸ��� �ʰ� �ٷ� �����ϱ� ������ �� ������.
//         using (StreamReader sr = new StreamReader(path)) 
//         using (CsvReader cr = new CsvReader(sr,csvConfig))
//         {
//             var records = cr.GetRecords<Player>(); //CsvHelper�� �ڵ����� ������ Player Ŭ������ �Ӽ����� ������ ��ü ����Ʈ�� ����
//             players = records.ToArray();
//         }
//
//         Debug.unityLogger.Log(players.Length.Equals(0) is false
//             ? $"Success Read Players ::: {path}"
//             : $"False Read Players ::: {path}");
//         
//         return players;
//     }*/
//     
//     // public Player.ToTsvPlayerInfo ConvertToTestInfo(Player.PlayerInfo _playerInfo)
//     // {
//     //     Player.ToTsvPlayerInfo testInfo = new Player.ToTsvPlayerInfo
//     //     {
//     //         Key = _playerInfo.key,
//     //         Name = _playerInfo.name,
//     //         CurrentHp = _playerInfo.currentHp,
//     //         MaxHp = _playerInfo.maxHp,
//     //         HasSkills = _playerInfo.hasSkills,
//     //         SkillSlot0 = _playerInfo.skillSlot0,
//     //         SkillSlot1 = _playerInfo.skillSlot1,
//     //         PositionX = _playerInfo.positionX,
//     //         PositionY = _playerInfo.positionY,
//     //         PositionZ = _playerInfo.positionZ,
//     //         AngleX = _playerInfo.angleX,
//     //         AngleY = _playerInfo.angleY,
//     //         AngleZ = _playerInfo.angleZ
//     //     };
//     //
//     //     return testInfo;
//     // }
// }