using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System.Xml;

public class CreateDatabaseUnity : MonoBehaviour {
	
	Exception ex;

	public void Start () {
		
		try
        {
			CreateDatabase.Main();
			//UserService.Register("Juani", "12", null);
			//UserService.Register("Juli", "12", null);
			RelationshipTypeDAO relationDAO = new RelationshipTypeDAO();
			relationDAO.createRelationshipTypes();
			//NHibernate.Cfg.Configuration cfg = new NHibernate.Cfg.Configuration();
			//cfg.Configure(ConfigurationBaseURL + "hibernate.cfg.txt");
			//cfg.Configure(Assembly.LoadFile(ConfigurationBaseURL + "mavaruonline.dll"), "Irrelevant.Assets.Database.hibernate.cfg.xml");
			//SchemaExport exporter = new SchemaExport(cfg);
			//exporter.Create(true, true); //borra y crea nuevamente las tablas en la base.
			
        }
        catch (Exception e)
        {
            ex = e;
        }
		
	}
	
	public void OnGUI()
    {
		if(ex != null)
			GUI.Label(new Rect(0,0,Screen.width,Screen.height), ex.ToString());
	
	}
	
	public static string ConfigurationBaseURL
    {
        get
        {
            if (Application.platform == RuntimePlatform.WindowsWebPlayer || Application.platform == RuntimePlatform.OSXWebPlayer)
                return Application.dataPath+"/";
            else
                return "file://" + Application.dataPath + "/../";
        }
    }
		
}
