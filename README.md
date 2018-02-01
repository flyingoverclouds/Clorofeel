# Clorofeel

Clorofeel ... the CLOud RObot ... for FEELings - a prototype of robot, built with Microsoft .NET technologies and Azure cloud.  
Clorofeel first public show was for **Innorobo 2011** in Lyon (France) http://innorobo.com/ on the Microsoft booth. 
Code evolve and was used as support for multiple technical session on TechDays France, Techdays Switzerland and SoftShake Switzerland.

Clorofeel brain is splitted in 3 parts : 
- Embedded : Roboard RB110 (486class PC with GPIO http://www.roboard.com/?page_id=1044), powered by a customized Windows XP installation. It execute code (C# on full .Net) that interact with hardware (camera,camera servo, motion servo, bumper switch, wifi, ...). It expose a WCF service thru ServiceBus Relay for remote monitoring/control.
- Cloud : Azure WebRole run cpu intensive Task (image analysis, face identification, video streaming on webapp) , ServiceBus relay for clorofeel remote command, Clorofeel Dashboard Silverlight app (display video stream from Clorofel + boxed  identified faces)  
- Mobile : Silverlight Windows Phone 7 app for clorofeel remote drive (caterpillar mode) + video monitoring relayed byt Azure services/service bus relay. 

Clorofeel is able to grab image from its cam, push it to a cloud service (remote brain part) to detect face (using Luxand FaceSDK https://www.luxand.com/facesdk/ ), and in return automatically move its cyclop's eye to the closest face : Clorofeel is tracking you. 

More technical details :
    - https://fr.scribd.com/document/71286656/2011-09-30-AzureCamp-Robotique
    - https://fr.slideshare.net/Developpeurs/concevoir-un-robot-avec-les-technologies-microsoft-18480304
    
