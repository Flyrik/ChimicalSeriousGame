# Chimical Serious Game

## 🎮 Lancement du projet

Le projet contient deux modes :
- **PCVR (Windows)** : pour casque VR connecté au PC
- **Casque autonome (Quest / Android)** : pour APK standalone

Le choix de la scène est automatique grâce à une scène de démarrage (Root).

---

## 🧠 Fonctionnement

Au lancement de l’application :
1. Unity charge la scène **Root**
2. Un script détecte la plateforme de build
3. La scène correspondante est chargée automatiquement :
   - Windows → MainPCVR
   - Android → MainAutonome

## 📦 Contenu implémenté
- Environnement industriel fonctionnel
- PNJ superviseur (guidage)
- Systèmes de sécurité obligatoire (étapes bloquantes)
- Barils de produits chimiques identifiables
- Système de tri avec conteneurs
- Feedback immédiat en cas d’erreur
- Bilan final de performance
- Cas d'erreur critique ou l'utilisation d'un extincteur est obligatoire
