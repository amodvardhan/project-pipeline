import ProjectDetails from '@/components/projects/ProjectDetails';

interface ProjectPageProps {
    params: Promise<{
        id: string;
    }>;
}

export default async function ProjectPage({ params }: ProjectPageProps) {
    const { id } = await params; // Await params before accessing properties
    return <ProjectDetails projectId={id} />;
}