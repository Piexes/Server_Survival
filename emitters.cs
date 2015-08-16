function servercmdemitTest(%client)
{
   %client.player.mountImage(speedSpellImage, 2);
}

datablock ParticleData(speedSpellParticle)
{
   dragCoefficient      = 0;
   gravityCoefficient   = 0.0;
   inheritedVelFactor   = 0.15;
   constantAcceleration = 0.0;
   lifetimeMS           = 1000;
   lifetimeVarianceMS   = 0;
   textureName          = "./Particles/speedSpell";
   colors[0]     = "0 0 0 1";
   sizes[0]      = 0.5;
   sizes[1]      = 0.6;
   times[0]      = 0.0;
   times[1]      = 1;
   useAlphaInv = false;
};
datablock ParticleEmitterData(speedSpellEmitter)
{
  ejectionPeriodMS = 10;
   periodVarianceMS = 0;
   ejectionVelocity = 1;
   ejectionOffset   = 1;
   velocityVariance = 0;
   thetaMin         = 89.9;
   thetaMax         = 90.1;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "speedSpellParticle";
   uiName = "Speedspell Trail";
};
datablock ShapeBaseImageData(speedSpellImage)
{
   shapeFile = "base/data/shapes/empty.dts";
   emap = false;
   mountPoint = $backslot;
   stateName[0] = "Ready"; //This is first
   stateTransitionOnTimeout[0] = "loopStart"; //Name of state to go to next.
   stateTimeoutValue[0] = 0.1; //Seconds for it to go to the next part of the loop.

   stateName[1] = "loopStart"; //Name of the state
   stateTransitionOnTimeout[1] = "loopEnd"; //next state you go to
   stateTimeoutValue[1] = 0.1; //Seconds til the next part of the loop
   stateEmitter[1] = "speedSpellEmitter"; //The emitted emitter datablock here.
   stateEmitterTime[1] = 1; //Time for the emitter to last, in seconds.

   stateName[2] = "loopEnd";
   stateWaitForTimeout[2] = 0;
   stateTransitionOnTimeout[2] = "loopStart";
   stateEmitterTime[2] = 1; //Emitter lifespan
   stateEmitter[2] = "speedSpellEmitter";
   stateTimeoutValue[2] = 1;
};
datablock ParticleData(snowCloudParticle)
{
   dragCoefficient      = 0;
   gravityCoefficient   = 0.0;
   inheritedVelFactor   = 0.15;
   constantAcceleration = 0.0;
   lifetimeMS           = 300;
   lifetimeVarianceMS   = 0;
   textureName          = "./Particles/speedSpell";
   colors[0]     = "0 0 0 1";
   sizes[0]      = 0.5;
   sizes[1]      = 0.6;
   times[0]      = 0.0;
   times[1]      = 1;
   useAlphaInv = false;
};
datablock ParticleEmitterData(snowCloudEmitter)
{
  ejectionPeriodMS = 2;
   periodVarianceMS = 0;
   ejectionVelocity = 0.1;
   ejectionOffset   = 0.35;
   velocityVariance = 0.1;
   thetaMin         = 89.9;
   thetaMax         = 90.1;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "snowCloudParticle";
   uiName = "Snowcloud";
};
datablock ShapeBaseImageData(snowCloudImage)
{
   shapeFile = "base/data/shapes/empty.dts";
   emap = false;
   mountPoint = $backslot;
   stateName[0] = "Ready"; //This is first
   stateTransitionOnTimeout[0] = "loopStart"; //Name of state to go to next.
   stateTimeoutValue[0] = 0.1; //Seconds for it to go to the next part of the loop.

   stateName[1] = "loopStart"; //Name of the state
   stateTransitionOnTimeout[1] = "loopEnd"; //next state you go to
   stateTimeoutValue[1] = 0.1; //Seconds til the next part of the loop
   stateEmitter[1] = "snowCloudEmitter"; //The emitted emitter datablock here.
   stateEmitterTime[1] = 1; //Time for the emitter to last, in seconds.

   stateName[2] = "loopEnd";
   stateWaitForTimeout[2] = 0;
   stateTransitionOnTimeout[2] = "loopStart";
   stateEmitterTime[2] = 1; //Emitter lifespan
   stateEmitter[2] = "snowCloudEmitter";
   stateTimeoutValue[2] = 1;
};
datablock particleData(teleportSpellParticle : speedSpellParticle)
{
   textureName = "./Particles/teleportSpell";
};
datablock particleemitterData(teleportSpellEmitter : speedSpellEmitter)
{
   particles = "teleportSpellParticle";
   uiName = "Teleport Trail";
};
datablock ShapeBaseImageData(teleportSpellImage : speedSpellImage)
{
   stateEmitter[1] = "teleportSpellEmitter";
   stateEmitter[2] = "teleportSpellEmitter";
};
datablock particleData(locationSpellParticle : speedSpellParticle)
{
   textureName = "./Particles/locationSpell";
};
datablock particleemitterData(locationSpellEmitter : speedSpellEmitter)
{
   particles = "locationSpellParticle";
   uiName = "Location Spell Trail";
};
datablock ShapeBaseImageData(locationSpellImage : speedSpellImage)
{
   stateEmitter[1] = "locationSpellEmitter";
   stateEmitter[2] = "locationSpellEmitter";
};
datablock particleData(surplusSpellParticle : speedSpellParticle)
{
   textureName = "./Particles/statBoostSpell";
};
datablock particleemitterData(surplusSpellEmitter : speedSpellEmitter)
{
   particles = "surplusSpellParticle";
   uiName = "Surplus Spell Trail";
};
datablock ShapeBaseImageData(surplusSpellImage : speedSpellImage)
{
   stateEmitter[1] = "surplusSpellEmitter";
   stateEmitter[2] = "surplusSpellEmitter";
};
datablock particleData(protectionSpellParticle : speedSpellParticle)
{
   textureName = "./Particles/protectionSpell";
};
datablock particleemitterData(protectionSpellEmitter : speedSpellEmitter)
{
   particles = "protectionSpellParticle";
   uiName = "Protection Spell Trail";
};
datablock ShapeBaseImageData(protectionSpellImage : speedSpellImage)
{
   stateEmitter[1] = "protectionSpellEmitter";
   stateEmitter[2] = "protectionSpellEmitter";
};
datablock particleData(fireballParticle : speedSpellParticle)
{
   textureName = "./Particles/fireball";
   lifetimeMS = 300;
};
datablock particleemitterData(fireballEmitter : speedSpellEmitter)
{
   particles = "fireballParticle";
   uiName = "Fireball Emitter";
   ejectionPeriodMS = 2;
   ejectionOffset = 0.1;
   periodVarianceMS = 0;
};
datablock ShapeBaseImageData(fireballImage : speedSpellImage)
{
   stateEmitter[1] = "fireballEmitter";
   stateEmitter[2] = "fireballEmitter";
};
datablock ParticleData(fireballExplosionParticle)
{
   dragCoefficient      = 8;
   gravityCoefficient   = 1;
   inheritedVelFactor   = 0.2;
   constantAcceleration = 0.0;
   lifetimeMS           = 700;
   lifetimeVarianceMS   = 400;
   textureName          = "./Particles/fireball";
   spinSpeed      = 10.0;
   spinRandomMin     = -50.0;
   spinRandomMax     = 50.0;
   colors[0]     = "0.9 0.9 0.9 0.3";
   colors[1]     = "0.9 0.5 0.6 0.0";
   sizes[0]      = 0.50;
   sizes[1]      = 0.75;

   useInvAlpha = true;
};
datablock ParticleEmitterData(fireballExplosionEmitter)
{
   ejectionPeriodMS = 1;
   periodVarianceMS = 0;
   ejectionVelocity = 2;
   velocityVariance = 1.0;
   ejectionOffset   = 0.0;
   thetaMin         = 89;
   thetaMax         = 90;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "fireballExplosionParticle";

   useEmitterColors = true;
   uiName = "Fireball Hit Dust";
};


datablock ParticleData(fireballExplosionRingParticle)
{
   dragCoefficient      = 8;
   gravityCoefficient   = -0.5;
   inheritedVelFactor   = 0.2;
   constantAcceleration = 0.0;
   lifetimeMS           = 50;
   lifetimeVarianceMS   = 35;
   textureName          = "./particles/teleportSpell";
   spinSpeed      = 500.0;
   spinRandomMin     = -500.0;
   spinRandomMax     = 500.0;
   colors[0]     = "1 1 0.0 0.9";
   colors[1]     = "0.9 0.0 0.0 0.0";
   sizes[0]      = 1;
   sizes[1]      = 0;

   useInvAlpha = false;
};
datablock ParticleEmitterData(fireballExplosionRingEmitter)
{
   lifeTimeMS = 50;

   ejectionPeriodMS = 3;
   periodVarianceMS = 0;
   ejectionVelocity = 0;
   velocityVariance = 0.0;
   ejectionOffset   = 0.0;
   thetaMin         = 89;
   thetaMax         = 90;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "fireballExplosionRingParticle";

   useEmitterColors = true;
   uiName = "Fireball Hit Smog";
};

datablock ExplosionData(fireballExplosion)
{
   //explosionShape = "";
   soundProfile = fireballHitSound;

   lifeTimeMS = 150;

   particleEmitter = fireballExplosionEmitter;
   particleDensity = 5;
   particleRadius = 0.2;

   emitter[0] = fireballExplosionRingEmitter;

   faceViewer     = true;
   explosionScale = "1 1 1";

   shakeCamera = false;
   camShakeFreq = "10.0 11.0 10.0";
   camShakeAmp = "1.0 1.0 1.0";
   camShakeDuration = 0.5;
   camShakeRadius = 10.0;

   // Dynamic light
   lightStartRadius = 2;
   lightEndRadius = 2;
   lightStartColor = "0.5 0.8 0.9";
   lightEndColor = "0 0 0";
};
function gameConnection::setEmitter(%client, %image, %timeout)
{
   if(!isObject(%client.player))
      return;
   %client.player.mountImage(%image, 3);
   if(%timeout)
      %client.player.schedule(%timeout, unmountImage, 3);
}