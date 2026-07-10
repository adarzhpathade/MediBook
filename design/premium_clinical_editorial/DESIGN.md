---
name: Premium Clinical Editorial
colors:
  surface: '#fdf8f8'
  surface-dim: '#ddd9d8'
  surface-bright: '#fdf8f8'
  surface-container-lowest: '#ffffff'
  surface-container-low: '#f7f3f2'
  surface-container: '#f1edec'
  surface-container-high: '#ebe7e6'
  surface-container-highest: '#e5e2e1'
  on-surface: '#1c1b1b'
  on-surface-variant: '#444748'
  inverse-surface: '#313030'
  inverse-on-surface: '#f4f0ef'
  outline: '#747878'
  outline-variant: '#c4c7c7'
  surface-tint: '#5f5e5e'
  primary: '#000000'
  on-primary: '#ffffff'
  primary-container: '#1c1b1b'
  on-primary-container: '#858383'
  inverse-primary: '#c8c6c5'
  secondary: '#804f6c'
  on-secondary: '#ffffff'
  secondary-container: '#fec0e1'
  on-secondary-container: '#7b4b67'
  tertiary: '#000000'
  on-tertiary: '#ffffff'
  tertiary-container: '#1d1b1a'
  on-tertiary-container: '#868381'
  error: '#ba1a1a'
  on-error: '#ffffff'
  error-container: '#ffdad6'
  on-error-container: '#93000a'
  primary-fixed: '#e5e2e1'
  primary-fixed-dim: '#c8c6c5'
  on-primary-fixed: '#1c1b1b'
  on-primary-fixed-variant: '#474646'
  secondary-fixed: '#ffd8eb'
  secondary-fixed-dim: '#f2b5d6'
  on-secondary-fixed: '#330d26'
  on-secondary-fixed-variant: '#653853'
  tertiary-fixed: '#e6e1df'
  tertiary-fixed-dim: '#cac6c3'
  on-tertiary-fixed: '#1d1b1a'
  on-tertiary-fixed-variant: '#484645'
  background: '#fdf8f8'
  on-background: '#1c1b1b'
  surface-variant: '#e5e2e1'
typography:
  display-lg:
    fontFamily: Manrope
    fontSize: 48px
    fontWeight: '700'
    lineHeight: 56px
    letterSpacing: -0.02em
  headline-lg:
    fontFamily: Manrope
    fontSize: 32px
    fontWeight: '700'
    lineHeight: 40px
    letterSpacing: -0.01em
  headline-lg-mobile:
    fontFamily: Manrope
    fontSize: 24px
    fontWeight: '700'
    lineHeight: 32px
  headline-md:
    fontFamily: Manrope
    fontSize: 24px
    fontWeight: '600'
    lineHeight: 32px
  body-lg:
    fontFamily: Manrope
    fontSize: 18px
    fontWeight: '400'
    lineHeight: 28px
  body-md:
    fontFamily: Manrope
    fontSize: 16px
    fontWeight: '400'
    lineHeight: 24px
  label-md:
    fontFamily: Manrope
    fontSize: 14px
    fontWeight: '600'
    lineHeight: 20px
  label-sm:
    fontFamily: Manrope
    fontSize: 12px
    fontWeight: '500'
    lineHeight: 16px
    letterSpacing: 0.02em
rounded:
  sm: 0.25rem
  DEFAULT: 0.5rem
  md: 0.75rem
  lg: 1rem
  xl: 1.5rem
  full: 9999px
spacing:
  unit: 8px
  container-padding: 40px
  card-gap: 24px
  section-margin: 64px
  sidebar-width: 280px
---

## Brand & Style

This design system is built for a premium healthcare SaaS environment that prioritizes practitioner well-being and cognitive clarity. The aesthetic shifts away from cold, sterile medical interfaces toward an **Editorial Modern** style—blending high-end lifestyle publication layouts with functional data visualization.

The brand personality is **composed, empathetic, and sophisticated**. It treats clinical data as a narrative, using massive whitespace and a warm color palette to reduce "alert fatigue" common in medical software. The emotional response should be one of calm control and professional prestige. 

Key stylistic pillars include:
- **Soft Geometry:** Extensive use of oversized radii to evoke a sense of safety and organic flow.
- **Color-Coded Hierarchy:** Using a sophisticated pastel palette to categorize information without overwhelming the user.
- **Airy Composition:** A "floating" UI approach where elements occupy distinct islands, separated by a generous warm background.

## Colors

The palette is anchored by a **warm cream background** (#F9F5EE) which acts as a softer, more premium alternative to pure white. This reduces eye strain during long shifts. 

- **Primary & Dark Surfaces:** Deep Charcoal (#111111) is reserved for structural navigation and high-contrast actions, providing a grounding force against the lighter canvas.
- **The Pastel Spectrum:** A collection of five specific tints—Pink, Yellow, Blue, Green, and Lavender—are used for semantic categorization (e.g., patient status, appointment types). These are never used for critical "danger" alerts; instead, they serve as gentle visual anchors.
- **Neutrality:** White (#FFFFFF) is used strictly for elevated cards and "floating" interactive surfaces to create a clear layer of depth above the cream background.

## Typography

The system utilizes **Manrope** exclusively to maintain a modern, technical, yet highly legible sans-serif profile. 

- **Scale:** The scale is generous. Large display headings drive the editorial feel and provide immediate orientation.
- **Weight:** Use `ExtraBold` (700) or `SemiBold` (600) for primary headers to create a strong visual anchor.
- **Letter Spacing:** Headings use slight negative tracking (-0.01em to -0.02em) to appear tighter and more "designed." Labels use positive tracking for increased legibility at small sizes.
- **Hierarchy:** Contrast is achieved through size and weight rather than color shifts, keeping the interface feeling clean and authoritative.

## Layout & Spacing

The layout follows a **Fluid-Floating model**. Instead of a rigid grid with visible borders, content is grouped into "floating islands" (cards) with massive surrounding whitespace.

- **Grid:** A 12-column system is used for desktop, but elements frequently break the grid or use "asymmetric balance" to maintain an editorial look.
- **Whitespace:** Use an 8px base unit, but lean into larger increments (40px, 64px) for outer margins and section breaks. 
- **The Sidebar:** A fixed-width, dark-surface sidebar acts as the primary anchor. It features a high inner radius (32px) on its right edge to separate it from the fluid content area.
- **Responsiveness:** On tablet/mobile, the "floating" cards stack vertically, and the 40px container padding reduces to 16px.

## Elevation & Depth

Depth in this system is achieved through **Soft Tonal Layering** and **Diffusion** rather than high-contrast shadows.

- **Layers:** 
    1. **Level 0 (Base):** Warm Cream (#F9F5EE) background.
    2. **Level 1 (Surface):** White (#FFFFFF) or Accent-Colored cards.
    3. **Level 2 (Interaction):** Floating action buttons or modals.
- **Shadows:** Use extremely soft, large-radius shadows (Blur: 40px-60px) with very low opacity (3-5%). The shadow color should be slightly tinted with the background hue (warm brown/grey) rather than pure black.
- **Depth Cues:** No inner shadows or bevels. Depth is purely "object-on-surface."

## Shapes

The shape language is dominated by **Exaggerated Rounding**. 

- **Cards:** Use a standard 28px - 32px corner radius. This creates a "friendly" and premium feel that softens the data-heavy nature of healthcare.
- **Interactive Elements:** Buttons, search bars, and tags must be **fully rounded (pill-shaped)**. This distinguishes them clearly from informational cards.
- **Consistency:** If an element is nested inside another, its corner radius should be slightly smaller to maintain visual concentricity.

## Components

- **Buttons:** Primary buttons are pill-shaped, using the dark #111111 surface with white text. Secondary buttons use pastel accents with high-transparency backgrounds.
- **Input Fields:** Search and data entry fields are pill-shaped with subtle 1px borders (#E5E0D5) or light cream fills.
- **Cards:** Elevated white surfaces with a 32px radius. Content inside should have generous 24px padding.
- **Chips/Tags:** Small pill-shaped containers used for categorization. Use the pastel palette for background fills (20% opacity) with the full-strength color for the text/icon.
- **Icons:** 2px stroke weight, rounded caps, and corners. Icons should always be placed within a circular or square-rounded container to maintain the "contained" aesthetic.
- **Timelines:** Use vertical dashed lines with circular nodes for schedule views, ensuring large touch targets for clinical events.