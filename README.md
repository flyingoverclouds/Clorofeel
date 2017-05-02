# Clorofeel

Clorofeel ... the CLOud RO ... for FEELings - a prototype of robot, fully built with Microsoft .NET technologies and Azure cloud
Clorofeel first public show was for **Innorobo 2011** in Lyon (France) http://innorobo.com/ on the Microsoft booth
Code evolve and was used as support for multiple technical session on TechDays France, Techdays Switzerland and SoftShare CH.

Clorofeel brain is splitted in 3 parts : 
- embedded : Roboard RB110 (486class PC with GPIO http://www.roboard.com/?page_id=1044) , powered by a customized Windows XP.It execute code (C# .Net FX) that interact with hardware (camera,camera servo, motion servo, bumper switch, wifi). It expose a WCF service thru ServiceBus Relay for remote monitoring/control.
- cloud : Azure WebRole (yes already in the cloud in 2011 :) ) run cpu intensive Task (image analysis, face identification) , ServiceBus relay for clorofeel remote command, clorofeel dashboard Silverlight App (display video stream from clorofel +boxed  identified faces)  
- mobile : Silverlight Windows Phone 7 app for clorofeel remote drive (caterpillar mode) + video monitoring relayed byt Azure services/service bus relay.

Clorofeel is able to grab image from its cam, call a cloud service (remote brain part) to detect face (using Luxand FaceSDK https://www.luxand.com/facesdk/ ), and in return automatically move its cyclop's eye to the closest face :) 

more technical details :
    - https://fr.scribd.com/document/71286656/2011-09-30-AzureCamp-Robotique
    - https://fr.slideshare.net/Developpeurs/concevoir-un-robot-avec-les-technologies-microsoft-18480304
    
