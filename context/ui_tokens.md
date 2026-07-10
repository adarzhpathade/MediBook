# MediBook UI Design Tokens

Version: 1.0

This document defines the official design tokens used throughout the MediBook application.

These tokens are the single source of truth for all UI implementation.

No page should introduce custom colors, spacing, typography, or shadows outside this system.

---

# Design Philosophy

MediBook follows a Premium Editorial Healthcare design language.

Characteristics:

- Warm and welcoming
- Calm clinical environment
- Large whitespace
- Soft floating cards
- Minimal visual noise
- Premium SaaS appearance
- Dashboard-first interface
- Modern accessibility

---

# Color Palette

## Base Colors

| Token | Value | Usage |
|---------|---------|---------|
| Background | #F9F5EE | Main application background |
| Surface | #FFFFFF | Cards |
| Surface Secondary | #F7F3EF | Secondary surfaces |
| Border | #E6DED4 | Borders |
| Divider | #ECE7E2 | Section dividers |

---

## Primary

| Token | Value |
|---------|---------|
| Primary | #111111 |
| Primary Hover | #222222 |
| Primary Active | #000000 |
| On Primary | #FFFFFF |

Used for

- Primary buttons
- Sidebar
- Active navigation
- Main CTA

---

## Secondary

| Token | Value |
|---------|---------|
| Secondary | #F4B7D8 |
| Secondary Hover | #EFA8CF |
| On Secondary | #111111 |

Used for

- Active menu
- Highlights
- Decorative accents

---

## Semantic Colors

### Success

Background

#DDF5E4

Text

#2E7D32

---

### Warning

Background

#FFF3CC

Text

#C78B00

---

### Error

Background

#FDE2E1

Text

#C62828

---

### Info

Background

#DCEAFF

Text

#2563EB

---

# Accent Palette

These colors are used for dashboard widgets only.

Pastel Pink

#F4B7D8

Pastel Blue

#BFD4FF

Pastel Green

#C6DBA3

Pastel Yellow

#F6D35E

Pastel Lavender

#DDD6FE

Never use accent colors for destructive actions.

---

# Sidebar

Background

#1B1B1B

Active Item

#F4B7D8

Text

#AFAFAF

Active Text

#FFFFFF

Hover

rgba(255,255,255,.06)

---

# Typography

Font Family

Manrope

Fallback

sans-serif

---

## Display

48px

Weight

700

Used for

Landing Hero

---

## H1

40px

700

Dashboard titles

---

## H2

32px

700

Page titles

---

## H3

24px

600

Section titles

---

## H4

20px

600

Card titles

---

## Body Large

18px

400

---

## Body

16px

400

---

## Small

14px

500

---

## Caption

12px

500

---

# Font Weights

Regular

400

Medium

500

SemiBold

600

Bold

700

---

# Border Radius

| Token | Value |
|---------|---------|
| xs | 6px |
| sm | 12px |
| md | 18px |
| lg | 24px |
| xl | 32px |
| pill | 999px |
| circle | 50% |

---

# Shadows

Soft

0 10px 30px rgba(0,0,0,.04)

Medium

0 20px 40px rgba(0,0,0,.05)

Large

0 30px 60px rgba(0,0,0,.06)

Hover

0 24px 48px rgba(0,0,0,.08)

---

# Spacing Scale

4

8

12

16

24

32

40

48

64

80

96

128

---

# Layout

Sidebar Width

280px

Content Padding

40px

Card Gap

24px

Section Gap

64px

Container Max Width

1600px

---

# Buttons

Height

52px

Radius

999px

Padding

24px

---

# Inputs

Height

54px

Radius

999px

Background

White

Border

1px Solid Border

---

# Cards

Radius

32px

Padding

24px

Background

White

Shadow

Soft

---

# Icons

Small

18px

Default

20px

Large

24px

Hero

32px

---

# Status Badges

Height

28px

Radius

999px

Font

12px Medium

Padding

12px

---

# Animations

Fast

150ms

Normal

250ms

Slow

350ms

Curve

ease-in-out

---

# Hover Effects

Cards

TranslateY(-2px)

Buttons

Scale(1.02)

Navigation

Background Fade

Inputs

Border Highlight

---

# Grid

Desktop

12 Columns

Tablet

8 Columns

Mobile

4 Columns

---

# Breakpoints

Mobile

<576px

Tablet

576px

Laptop

992px

Desktop

1200px

Large Desktop

1400px

---

# Z Index

Navbar

100

Sidebar

200

Dropdown

500

Modal

1000

Toast

1100

Tooltip

1200

Loader

9999

---

# Bootstrap Theme

Bootstrap should inherit these values.

Custom CSS variables should override Bootstrap defaults.

No Bootstrap component should use default blue colors.

---

# Dark Mode

Not included in Version 1.

Entire application uses the Light Premium Editorial theme.